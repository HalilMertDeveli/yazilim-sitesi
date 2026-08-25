using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portfolio.Models;

namespace Portfolio.Pages;

public class IndexModel : PageModel
{
    private static readonly HttpClient SharedClient = CreateClient();
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
    {
        _logger = logger;
        var configured = configuration["Site:PublicBaseUrl"]?.Trim().TrimEnd('/');
        if (!string.IsNullOrWhiteSpace(configured))
        {
            SiteUrl = configured;
            if (Uri.TryCreate(configured, UriKind.Absolute, out var uri) && !string.IsNullOrWhiteSpace(uri.Host))
            {
                SiteHost = uri.Host;
            }
        }
    }

    public IReadOnlyList<GitHubRepoView> Repos { get; private set; } = BuildFallback();
    public IReadOnlyList<LanguageStat> LanguageStats { get; private set; } = FallbackLanguageStats;
    public int PublicRepoCount { get; private set; } = 36;
    public string GitHubProfileUrl { get; } = "https://github.com/HalilMertDeveli";
    public string GitHubReposUrl { get; } = "https://github.com/HalilMertDeveli?tab=repositories";
    public string SiteUrl { get; private set; } = "https://www.halilmertdeveli.com.tr";
    public string SiteHost { get; private set; } = "www.halilmertdeveli.com.tr";
    public bool ReposFromApi { get; private set; }

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        var merged = new List<GitHubRepoView> { ThisPortfolioCard };

        try
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                "https://api.github.com/users/HalilMertDeveli/repos?per_page=100&sort=updated");
            using var response = await SharedClient.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("GitHub API returned {Status}", response.StatusCode);
                return;
            }

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

            var repos = new List<GitHubRepoView>();
            var languageCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in document.RootElement.EnumerateArray())
            {
                if (item.TryGetProperty("fork", out var fork) && fork.GetBoolean())
                {
                    continue;
                }

                var name = item.GetProperty("name").GetString() ?? string.Empty;
                if (string.Equals(name, "HalilMertDeveli", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (string.Equals(name, ThisPortfolioCard.Name, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var description = item.TryGetProperty("description", out var desc) && desc.ValueKind != JsonValueKind.Null
                    ? desc.GetString()
                    : null;
                var language = item.TryGetProperty("language", out var lang) && lang.ValueKind != JsonValueKind.Null
                    ? lang.GetString()
                    : null;
                var stars = item.TryGetProperty("stargazers_count", out var starEl) ? starEl.GetInt32() : 0;
                var url = item.GetProperty("html_url").GetString() ?? GitHubProfileUrl;
                var updated = item.TryGetProperty("pushed_at", out var pushed) && pushed.ValueKind == JsonValueKind.String
                    ? pushed.GetString()?[..Math.Min(10, pushed.GetString()!.Length)]
                    : null;

                if (!string.IsNullOrWhiteSpace(language))
                {
                    languageCounts[language] = languageCounts.GetValueOrDefault(language) + 1;
                }

                repos.Add(new GitHubRepoView
                {
                    Name = name,
                    Url = url,
                    Description = LocalizeDescription(name, description),
                    Language = language,
                    Stars = stars,
                    UpdatedAt = updated
                });
            }

            PublicRepoCount = repos.Count;
            LanguageStats = BuildLanguageStats(languageCounts);

            merged.AddRange(
                repos
                    .OrderByDescending(r => FeaturedRank(r.Name))
                    .ThenByDescending(r => r.Stars)
                    .ThenByDescending(r => r.UpdatedAt)
                    .Take(9));

            Repos = merged;
            ReposFromApi = true;
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            _logger.LogWarning(ex, "GitHub repos could not be loaded; using curated fallback.");
            Repos = BuildFallback();
        }
    }

    private static IReadOnlyList<LanguageStat> BuildLanguageStats(Dictionary<string, int> counts)
    {
        if (counts.Count == 0)
        {
            return FallbackLanguageStats;
        }

        var total = counts.Values.Sum();
        return counts
            .OrderByDescending(kv => kv.Value)
            .Take(6)
            .Select(kv => new LanguageStat
            {
                Name = NormalizeLanguageLabel(kv.Key),
                Count = kv.Value,
                Percent = total == 0 ? 0 : (int)Math.Round(kv.Value * 100.0 / total)
            })
            .ToList();
    }

    private static string NormalizeLanguageLabel(string language) => language switch
    {
        "C#" => "C# / .NET",
        "Dart" => "Dart / Flutter",
        "C++" => "C++ (Flutter native)",
        _ => language
    };

    private static IReadOnlyList<GitHubRepoView> BuildFallback()
    {
        var list = new List<GitHubRepoView> { ThisPortfolioCard };
        list.AddRange(FeaturedFallback);
        return list;
    }

    private static HttpClient CreateClient()
    {
        var client = new HttpClient { Timeout = TimeSpan.FromSeconds(6) };
        client.DefaultRequestHeaders.UserAgent.ParseAdd("HalilMertDeveli-Portfolio");
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
        return client;
    }

    private static int FeaturedRank(string name) => name.ToLowerInvariant() switch
    {
        "clearpay" => 100,
        "taskmanagementsystem" => 90,
        "personal-finance-tracker" => 85,
        "fluttervpnapp" => 82,
        "vpnappwithcompose" => 80,
        "asp.net-app-for-led" => 78,
        "led-teknik-destek" => 76,
        "etradeappwithvideos-" => 72,
        "bankappasp" => 70,
        "asp-net-e-trade" => 65,
        "reelfindingpeopleandoridapp" => 60,
        "asp.net-learning-porject" => 50,
        "identitycourse" => 45,
        "nlayerdapp" => 40,
        _ => 0
    };

    private static string? LocalizeDescription(string name, string? apiDescription) => name.ToLowerInvariant() switch
    {
        "clearpay" => "Mülakat reposu: ASP.NET Core 8 + Flutter dijital cüzdan. Bakiye UPDATE yok — double-entry SQL ledger, idempotent transfer, mock banka gateway.",
        "taskmanagementsystem" => "Bitirme projesi: Onion mimarili görev yönetimi. Admin/Member rolleri, MediatR, cookie auth, raporlama.",
        "asp.net-app-for-led" => "LED paneller için ASP.NET Core web uygulaması — sahada kullanılan teknik vitrin.",
        "bankappasp" => "ASP.NET ile bankacılık senaryolarına odaklı uygulama denemesi.",
        "asp-net-e-trade" => "ASP.NET tabanlı e-ticaret vitrin / alışveriş akışı.",
        "led-teknik-destek" => "Colorlight, Novastar ve Huudi için LED teknik destek sitesi.",
        "personal-finance-tracker" => "Flutter mobil: kişisel finans takip uygulaması.",
        "fluttervpnapp" => "Flutter ile tasarlayıp kodladığım VPN uygulama denemesi.",
        "vpnappwithcompose" => "Jetpack Compose (Kotlin) ile VPN arayüz / akış denemesi.",
        "vpnappwithtutorial" => "VPN uygulaması öğrenme / tutorial serisi (Java).",
        "etradeappwithvideos-" => "Flutter ile e-ticaret mobil uygulama çalışması.",
        "reelfindingpeopleandoridapp" => "Android Kotlin/XML ile reel / kişi bulma uygulaması.",
        "asp.net-learning-porject" => "ASP.NET öğrenme sürecindeki uygulamalı çalışmalar.",
        "identitycourse" => "ASP.NET Identity ile kimlik ve yetkilendirme pratikleri.",
        "nlayerdapp" => "C# ile n-katmanlı mimari ve Windows uygulama yapısı.",
        "kisisel-portfolyo" => "Bu site: koyu temalı, animasyonlu tek sayfa ASP.NET Core 8 portföy / CV.",
        _ => string.IsNullOrWhiteSpace(apiDescription) ? "GitHub üzerinde açık kaynak çalışma." : apiDescription
    };

    private static readonly GitHubRepoView ThisPortfolioCard = new()
    {
        Name = "kisisel-portfolyo",
        Url = "https://github.com/HalilMertDeveli/kisisel-portfolyo",
        Description = "Bu site: koyu temalı, animasyonlu tek sayfa ASP.NET Core 8 portföy / CV. Domain + hosting ile canlıya alınacak.",
        Language = "C#",
        Stars = 0,
        UpdatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd")
    };

    private static readonly IReadOnlyList<LanguageStat> FallbackLanguageStats =
    [
        new() { Name = "C# / .NET", Count = 10, Percent = 28 },
        new() { Name = "Kotlin", Count = 5, Percent = 14 },
        new() { Name = "Java", Count = 5, Percent = 14 },
        new() { Name = "C++ (Flutter native)", Count = 6, Percent = 17 },
        new() { Name = "Dart / Flutter", Count = 3, Percent = 8 },
        new() { Name = "HTML", Count = 3, Percent = 8 }
    ];

    private static readonly IReadOnlyList<GitHubRepoView> FeaturedFallback =
    [
        new()
        {
            Name = "clearpay",
            Url = "https://github.com/HalilMertDeveli/clearpay",
            Description = "Mülakat reposu: ASP.NET Core 8 + Flutter dijital cüzdan. Bakiye UPDATE yok — double-entry SQL ledger, idempotent transfer, mock banka gateway.",
            Language = "C#",
            Stars = 1,
            UpdatedAt = "2026-08-18"
        },
        new()
        {
            Name = "TaskManagementSystem",
            Url = "https://github.com/HalilMertDeveli/TaskManagementSystem",
            Description = "Bitirme projesi: Onion mimarili görev yönetimi. Admin/Member rolleri, MediatR, cookie auth, raporlama.",
            Language = "C#",
            Stars = 0,
            UpdatedAt = "2026-05-12"
        },
        new()
        {
            Name = "ASP.NET-APP-FOR-LED",
            Url = "https://github.com/HalilMertDeveli/ASP.NET-APP-FOR-LED",
            Description = "LED paneller için ASP.NET Core web uygulaması — sahada kullanılan teknik vitrin.",
            Language = "C#",
            Stars = 0,
            UpdatedAt = "2026-04-17"
        },
        new()
        {
            Name = "led-teknik-destek",
            Url = "https://github.com/HalilMertDeveli/led-teknik-destek",
            Description = "Colorlight, Novastar ve Huudi için LED teknik destek sitesi.",
            Language = "HTML",
            Stars = 0,
            UpdatedAt = "2026-08-12"
        },
        new()
        {
            Name = "BankAppAsp",
            Url = "https://github.com/HalilMertDeveli/BankAppAsp",
            Description = "ASP.NET ile bankacılık senaryolarına odaklı uygulama denemesi.",
            Language = "C#",
            Stars = 0,
            UpdatedAt = "2024-10-13"
        },
        new()
        {
            Name = "ASP-NET-E-Trade",
            Url = "https://github.com/HalilMertDeveli/ASP-NET-E-Trade",
            Description = "ASP.NET tabanlı e-ticaret vitrin / alışveriş akışı.",
            Language = "HTML",
            Stars = 0,
            UpdatedAt = "2024-07-01"
        },
        new()
        {
            Name = "personal-Finance-Tracker",
            Url = "https://github.com/HalilMertDeveli/personal-Finance-Tracker",
            Description = "Flutter mobil: kişisel finans takip uygulaması.",
            Language = "Dart",
            Stars = 0,
            UpdatedAt = "2024-09-07"
        },
        new()
        {
            Name = "FlutterVpnApp",
            Url = "https://github.com/HalilMertDeveli/FlutterVpnApp",
            Description = "Flutter ile tasarlayıp kodladığım VPN uygulama denemesi.",
            Language = "Dart",
            Stars = 0,
            UpdatedAt = "2023-10-13"
        },
        new()
        {
            Name = "VPNAppWithCompose",
            Url = "https://github.com/HalilMertDeveli/VPNAppWithCompose",
            Description = "Jetpack Compose (Kotlin) ile VPN arayüz / akış denemesi.",
            Language = "Kotlin",
            Stars = 0,
            UpdatedAt = "2023-10-09"
        }
    ];
}
