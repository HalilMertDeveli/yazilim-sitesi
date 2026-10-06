using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Portfolio.Models;

namespace Portfolio.Services;

/// <summary>
/// Reads public repository data for the profile from the GitHub REST API.
/// Results are cached in memory so page views do not burn the unauthenticated
/// rate limit (60 req/h per IP), and any failure falls back to a static snapshot.
/// An optional server-side token (GitHub:Token / env GitHub__Token) raises the limit;
/// it is never sent to the browser.
/// </summary>
public sealed class GitHubService
{
    public const string UserName = "HalilMertDeveli";
    private const string CacheKey = "github-snapshot";
    private static readonly TimeSpan LiveTtl = TimeSpan.FromMinutes(30);
    private static readonly TimeSpan FallbackTtl = TimeSpan.FromMinutes(5);

    private readonly HttpClient _http;
    private readonly IMemoryCache _cache;
    private readonly ILogger<GitHubService> _logger;
    private static readonly SemaphoreSlim _gate = new(1, 1);

    public GitHubService(HttpClient http, IMemoryCache cache, IConfiguration configuration, ILogger<GitHubService> logger)
    {
        _http = http;
        _cache = cache;
        _logger = logger;

        _http.BaseAddress = new Uri(configuration["GitHub:ApiBaseUrl"] ?? "https://api.github.com/");
        _http.Timeout = TimeSpan.FromSeconds(4);
        _http.DefaultRequestHeaders.UserAgent.ParseAdd("halilmertdeveli.com.tr-portfolio");
        _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
        _http.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");

        var token = configuration["GitHub:Token"];
        if (!string.IsNullOrWhiteSpace(token))
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Trim());
        }
    }

    public async Task<GitHubSnapshot> GetSnapshotAsync(CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(CacheKey, out GitHubSnapshot? cached) && cached is not null)
        {
            return cached;
        }

        await _gate.WaitAsync(cancellationToken);
        try
        {
            if (_cache.TryGetValue(CacheKey, out cached) && cached is not null)
            {
                return cached;
            }

            var live = await TryFetchAsync(cancellationToken);
            var snapshot = live ?? Fallback;
            _cache.Set(CacheKey, snapshot, live is null ? FallbackTtl : LiveTtl);
            return snapshot;
        }
        finally
        {
            _gate.Release();
        }
    }

    private async Task<GitHubSnapshot?> TryFetchAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var response = await _http.GetAsync($"users/{UserName}/repos?per_page=100&sort=pushed&type=owner", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("GitHub API returned {Status}; using fallback snapshot.", (int)response.StatusCode);
                return null;
            }

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

            var repos = new List<GitHubRepoInfo>();
            foreach (var item in document.RootElement.EnumerateArray())
            {
                if (GetBool(item, "fork") || GetBool(item, "private") || GetBool(item, "archived"))
                {
                    continue;
                }

                var name = GetString(item, "name");
                if (string.IsNullOrEmpty(name) || name.Equals(UserName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                repos.Add(new GitHubRepoInfo(
                    name,
                    GetString(item, "html_url") ?? $"https://github.com/{UserName}/{name}",
                    GetString(item, "description"),
                    GetString(item, "language"),
                    item.TryGetProperty("stargazers_count", out var stars) && stars.ValueKind == JsonValueKind.Number ? stars.GetInt32() : 0,
                    DateTimeOffset.TryParse(GetString(item, "pushed_at"), out var pushed) ? pushed : null));
            }

            if (repos.Count == 0)
            {
                return null;
            }

            return Build(repos, isLive: true);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException or InvalidOperationException)
        {
            _logger.LogWarning(ex, "GitHub data could not be loaded; using fallback snapshot.");
            return null;
        }
    }

    private static GitHubSnapshot Build(IReadOnlyList<GitHubRepoInfo> repos, bool isLive)
    {
        var languageCounts = repos
            .Select(r => NormalizeLanguage(r.Language))
            .Where(l => l is not null)
            .GroupBy(l => l!)
            .Select(g => (Name: g.Key, Count: g.Count()))
            .OrderByDescending(x => x.Count)
            .ToList();
        var total = languageCounts.Sum(x => x.Count);

        return new GitHubSnapshot
        {
            PublicRepos = repos.Count,
            Languages = languageCounts
                .Take(5)
                .Select(x => new LanguageShare(x.Name, x.Count, total == 0 ? 0 : (int)Math.Round(x.Count * 100.0 / total)))
                .ToList(),
            RecentRepos = repos
                .Where(r => r.PushedAt is not null && !r.Name.Equals("yazilim-sitesi", StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(r => r.PushedAt)
                .Take(4)
                .ToList(),
            ByName = repos.ToDictionary(r => r.Name, StringComparer.OrdinalIgnoreCase),
            LastPush = repos.Max(r => r.PushedAt),
            IsLive = isLive
        };
    }

    /// <summary>
    /// GitHub classifies Flutter repositories as C++ because of their desktop runner folders,
    /// so those are folded into Flutter / Dart; stylesheet languages are folded into web.
    /// </summary>
    private static string? NormalizeLanguage(string? language) => language switch
    {
        null or "" => null,
        "C#" => "C# / .NET",
        "Dart" or "C++" => "Flutter / Dart",
        "HTML" or "CSS" or "SCSS" or "JavaScript" or "TypeScript" => "Web",
        _ => language
    };

    private static bool GetBool(JsonElement item, string name) =>
        item.TryGetProperty(name, out var value) && value.ValueKind == JsonValueKind.True;

    private static string? GetString(JsonElement item, string name) =>
        item.TryGetProperty(name, out var value) && value.ValueKind == JsonValueKind.String ? value.GetString() : null;

    private static GitHubRepoInfo Repo(string name, string language, string pushed, int stars = 0) =>
        new(name, $"https://github.com/{UserName}/{name}", null, language, stars, DateTimeOffset.Parse(pushed));

    /// <summary>Snapshot of the public profile taken on 2026-10-06; used when the API is unreachable.</summary>
    private static readonly GitHubSnapshot Fallback = Build(
    [
        Repo("mevora", "Dart", "2026-10-06T06:47:16Z"),
        Repo("ASP.NET-APP-FOR-LED", "C#", "2026-08-27T08:19:51Z"),
        Repo("yazilim-sitesi", "CSS", "2026-08-26T09:39:29Z"),
        Repo("clearpay", "C#", "2026-08-18T08:06:48Z", 1),
        Repo("led-teknik-destek", "HTML", "2026-08-12T12:05:36Z"),
        Repo("IdentityCourse", "C#", "2026-05-18T12:48:03Z"),
        Repo("BTKHighLevelKotlinCourse", "Kotlin", "2026-05-18T11:52:09Z"),
        Repo("TaskManagementSystem", "SCSS", "2026-05-12T05:57:05Z"),
        Repo("AdvacedKotlinCourse", "Kotlin", "2026-05-11T11:57:52Z"),
        Repo("BankAppAsp", "C#", "2024-10-13T11:15:18Z"),
        Repo("personal-Finance-Tracker", "Dart", "2024-09-07T15:14:06Z"),
        Repo("ASP-NET-E-Trade", "HTML", "2024-07-01T17:21:48Z"),
        Repo("Sqflite-Fltuter-App", "C++", "2024-06-30T11:19:04Z"),
        Repo("ASP.NET-Learning-Porject", "SCSS", "2024-06-07T15:46:24Z", 1),
        Repo("entity_framework", "C#", "2024-05-15T09:32:55Z"),
        Repo("EntityCoreCRUD", "C#", "2024-03-24T11:30:12Z"),
        Repo("fundemantal_of_biomedical_proejct", "C++", "2024-03-07T02:36:11Z"),
        Repo("FirebaseChatApp", "Dart", "2023-12-07T13:34:00Z"),
        Repo("BTKCBackendCourse", "C#", "2023-12-07T09:54:51Z"),
        Repo("GoogleAuthOperationWithFlutterGoogleWorkspace", "C++", "2023-11-04T11:37:09Z"),
        Repo("AllDesingPatternInBtkC-", "C#", "2023-10-13T18:42:57Z"),
        Repo("FlutterVpnApp", "C++", "2023-10-13T15:33:41Z"),
        Repo("VPNAppWithCompose", "Kotlin", "2023-10-09T08:25:19Z"),
        Repo("JDBC", "Java", "2023-09-18T10:51:01Z"),
        Repo("NLayerdApp", "C#", "2023-09-12T07:44:08Z"),
        Repo("GoogleFlutterCourse", "C++", "2023-09-11T11:51:40Z"),
        Repo("Windows-app.", "C#", "2023-08-29T14:00:09Z"),
        Repo("SpringYoutubeCourse", "Java", "2023-08-21T16:28:13Z"),
        Repo("ReactLerningProject", "JavaScript", "2023-08-19T16:57:10Z"),
        Repo("VPNAppWithTutorial", "Java", "2023-08-18T14:16:42Z"),
        Repo("SpirngBootWithDoc", "Java", "2023-08-13T17:41:22Z"),
        Repo("FirstNextjsProject", "HTML", "2023-08-11T17:56:32Z"),
        Repo("SpringBootProject", "Java", "2023-07-26T12:22:06Z"),
        Repo("MVVMGettingLogic", "Kotlin", "2023-07-23T13:11:30Z"),
        Repo("RESTAPI-Usages", "C++", "2023-05-29T07:35:44Z"),
        Repo("ReelFindingPeopleAndoridApp", "Kotlin", "2023-02-10T10:51:38Z"),
        Repo("ETradeAppWithVideos-", "Dart", "2023-02-10T10:42:29Z")
    ], isLive: false);
}
