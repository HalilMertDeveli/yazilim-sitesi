using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portfolio.Models;

namespace Portfolio.Pages;

public class IndexModel : PageModel
{
    private static readonly HttpClient SharedClient = CreateClient();
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public IReadOnlyList<GitHubRepoView> Repos { get; private set; } = FeaturedFallback;
    public string GitHubProfileUrl { get; } = "https://github.com/HalilMertDeveli";
    public bool ReposFromApi { get; private set; }

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
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

            if (repos.Count == 0)
            {
                return;
            }

            Repos = repos
                .OrderByDescending(r => FeaturedRank(r.Name))
                .ThenByDescending(r => r.Stars)
                .ThenByDescending(r => r.UpdatedAt)
                .Take(8)
                .ToList();
            ReposFromApi = true;
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            _logger.LogWarning(ex, "GitHub repos could not be loaded; using curated fallback.");
        }
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
        "asp.net-app-for-led" => 80,
        "bankappasp" => 70,
        "asp-net-e-trade" => 65,
        "led-teknik-destek" => 60,
        "personal-finance-tracker" => 55,
        "asp.net-learning-porject" => 50,
        "identitycourse" => 45,
        "nlayerdapp" => 40,
        _ => 0
    };

    private static string? LocalizeDescription(string name, string? apiDescription) => name.ToLowerInvariant() switch
    {
        "clearpay" => "ASP.NET Core 8 dijital cüzdan: idempotent transferler, SQL ledger ve mock banka gateway.",
        "taskmanagementsystem" => "Onion mimarili görev yönetim sistemi — ASP.NET Core MVC ile katmanlı yapı.",
        "asp.net-app-for-led" => "LED paneller için ASP.NET Core web uygulaması.",
        "bankappasp" => "ASP.NET ile bankacılık senaryolarına odaklı uygulama denemesi.",
        "asp-net-e-trade" => "ASP.NET tabanlı e-ticaret vitrin / alışveriş akışı.",
        "led-teknik-destek" => "Colorlight, Novastar ve Huudi için LED teknik destek sitesi.",
        "personal-finance-tracker" => "Flutter ile kişisel finans takip uygulaması.",
        "asp.net-learning-porject" => "ASP.NET öğrenme sürecindeki uygulamalı çalışmalar.",
        "identitycourse" => "ASP.NET Identity ile kimlik ve yetkilendirme pratikleri.",
        "nlayerdapp" => "C# ile n-katmanlı mimari ve Windows uygulama yapısı.",
        _ => string.IsNullOrWhiteSpace(apiDescription) ? "GitHub üzerinde açık kaynak çalışma." : apiDescription
    };

    private static readonly IReadOnlyList<GitHubRepoView> FeaturedFallback =
    [
        new()
        {
            Name = "clearpay",
            Url = "https://github.com/HalilMertDeveli/clearpay",
            Description = "ASP.NET Core 8 dijital cüzdan: idempotent transferler, SQL ledger ve mock banka gateway.",
            Language = "C#",
            Stars = 1,
            UpdatedAt = "2026-08-18"
        },
        new()
        {
            Name = "TaskManagementSystem",
            Url = "https://github.com/HalilMertDeveli/TaskManagementSystem",
            Description = "Onion mimarili görev yönetim sistemi — ASP.NET Core MVC ile katmanlı yapı.",
            Language = "C#",
            Stars = 0,
            UpdatedAt = "2026-05-12"
        },
        new()
        {
            Name = "ASP.NET-APP-FOR-LED",
            Url = "https://github.com/HalilMertDeveli/ASP.NET-APP-FOR-LED",
            Description = "LED paneller için ASP.NET Core web uygulaması.",
            Language = "C#",
            Stars = 0,
            UpdatedAt = "2026-04-17"
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
            Name = "led-teknik-destek",
            Url = "https://github.com/HalilMertDeveli/led-teknik-destek",
            Description = "Colorlight, Novastar ve Huudi için LED teknik destek sitesi.",
            Language = "HTML",
            Stars = 0,
            UpdatedAt = "2026-08-12"
        },
        new()
        {
            Name = "personal-Finance-Tracker",
            Url = "https://github.com/HalilMertDeveli/personal-Finance-Tracker",
            Description = "Flutter ile kişisel finans takip uygulaması.",
            Language = "Dart",
            Stars = 0,
            UpdatedAt = "2024-09-07"
        },
        new()
        {
            Name = "ASP.NET-Learning-Porject",
            Url = "https://github.com/HalilMertDeveli/ASP.NET-Learning-Porject",
            Description = "ASP.NET öğrenme sürecindeki uygulamalı çalışmalar.",
            Language = "SCSS",
            Stars = 1,
            UpdatedAt = "2024-06-07"
        }
    ];
}
