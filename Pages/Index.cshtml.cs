using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portfolio.Models;
using Portfolio.Services;

namespace Portfolio.Pages;

public class IndexModel : PageModel
{
    private readonly GitHubService _gitHub;

    public IndexModel(GitHubService gitHub, IConfiguration configuration)
    {
        _gitHub = gitHub;
        SiteUrl = (configuration["Site:PublicBaseUrl"] ?? "https://halilmertdeveli.com.tr").Trim().TrimEnd('/');
        ContactEmail = configuration["Site:ContactEmail"] ?? PortfolioContent.Email;
    }

    public SiteLanguage Lang { get; private set; } = SiteLanguage.Tr;
    public GitHubSnapshot GitHub { get; private set; } = null!;
    public string SiteUrl { get; }
    public string ContactEmail { get; }

    public bool IsEn => Lang == SiteLanguage.En;
    public string LangCode => IsEn ? "en" : "tr";
    public string HomePath => IsEn ? "/en" : "/";
    public string CanonicalUrl => IsEn ? $"{SiteUrl}/en" : $"{SiteUrl}/";

    public async Task OnGetAsync(string? lang, CancellationToken cancellationToken)
    {
        Lang = string.Equals(lang, "en", StringComparison.OrdinalIgnoreCase) ? SiteLanguage.En : SiteLanguage.Tr;
        GitHub = await _gitHub.GetSnapshotAsync(cancellationToken);
    }

    public string PageTitle => T(
        "Halil Mert Develi — Yazılım Mühendisi · ASP.NET Core & Flutter",
        "Halil Mert Develi — Software Engineer · ASP.NET Core & Flutter");

    public string MetaDescription => T(
        "İstanbul'da yazılım mühendisi Halil Mert Develi'nin portföyü: ASP.NET Core backend sistemleri, Flutter mobil uygulamalar ve LED ekran sektörü için yazılımlar. Mevora, LED-COM ve LED Support Bot vaka çalışmaları.",
        "Portfolio of Halil Mert Develi, a software engineer in Istanbul building ASP.NET Core backends, Flutter mobile apps and software for the LED display industry. Case studies: Mevora, LED-COM and LED Support Bot.");

    /// <summary>schema.org Person + WebSite graph. Serialized with the default encoder, which escapes &lt; and &gt;.</summary>
    public string JsonLd => JsonSerializer.Serialize(new Dictionary<string, object>
    {
        ["@context"] = "https://schema.org",
        ["@graph"] = new object[]
        {
            new Dictionary<string, object>
            {
                ["@type"] = "Person",
                ["@id"] = $"{SiteUrl}/#person",
                ["name"] = PortfolioContent.FullName,
                ["url"] = $"{SiteUrl}/",
                ["image"] = $"{SiteUrl}{PortfolioContent.Portrait}",
                ["jobTitle"] = T("Yazılım Mühendisi", "Software Engineer"),
                ["email"] = $"mailto:{ContactEmail}",
                ["address"] = new Dictionary<string, object>
                {
                    ["@type"] = "PostalAddress",
                    ["addressLocality"] = "İstanbul",
                    ["addressCountry"] = "TR"
                },
                ["knowsAbout"] = new[] { "ASP.NET Core", ".NET", "C#", "Flutter", "Dart", "Firebase", "PostgreSQL", "Clean Architecture", "Backend development", "Mobile development" },
                ["sameAs"] = new[] { PortfolioContent.GitHubUrl, PortfolioContent.LinkedInUrl }
            },
            new Dictionary<string, object>
            {
                ["@type"] = "WebSite",
                ["@id"] = $"{SiteUrl}/#website",
                ["url"] = $"{SiteUrl}/",
                ["name"] = PortfolioContent.FullName,
                ["inLanguage"] = new[] { "tr-TR", "en" },
                ["author"] = new Dictionary<string, object> { ["@id"] = $"{SiteUrl}/#person" }
            }
        }
    });

    public string T(string tr, string en) => IsEn ? en : tr;

    public string T(Text text) => text.Get(Lang);

    public string FormatMonth(DateTimeOffset? date) =>
        date is null
            ? "—"
            : date.Value.ToString("MMM yyyy", System.Globalization.CultureInfo.GetCultureInfo(IsEn ? "en-GB" : "tr-TR"));

    public string ToneClass(StatusTone tone) => tone switch
    {
        StatusTone.Active => "badge--active",
        StatusTone.Live => "badge--live",
        StatusTone.Private => "badge--private",
        StatusTone.Open => "badge--open",
        _ => "badge--neutral"
    };
}

/// <summary>View model for one case study partial.</summary>
public sealed record CaseStudyView(IndexModel Page, CaseStudy Study, int Index);
