namespace Portfolio.Models;

/// <summary>A piece of copy in both site languages.</summary>
public sealed record Text(string Tr, string En)
{
    public string Get(SiteLanguage lang) => lang == SiteLanguage.En ? En : Tr;
}

public enum SiteLanguage
{
    Tr,
    En
}

public enum StatusTone
{
    Active,
    Live,
    Private,
    Open,
    Neutral
}

public sealed record StatusBadge(Text Label, StatusTone Tone);

public sealed record ProjectLink(Text Label, string Url, bool External = true);

public sealed record Highlight(Text Title, Text Body);

public sealed record ProjectImage(string Src, Text Alt, int Width, int Height);

/// <summary>A headline number on a case study. Value is rendered in full and counted up by script.</summary>
public sealed record Metric(int Value, string Suffix, Text Label);

/// <summary>A repository with commits co-authored by Claude.</summary>
public sealed record ClaudeProject(Text Name, int Commits, Text Summary, string Href, bool IsPrivate = false);

/// <summary>A smaller or earlier repository, listed as one line.</summary>
public sealed record ArchiveRepo(string RepoName, string Language, Text Summary, string Tags);

/// <summary>Flagship project rendered as a case study.</summary>
public sealed class CaseStudy
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required Text Tagline { get; init; }
    public required IReadOnlyList<Metric> Metrics { get; init; }

    /// <summary>CSS custom property holding the project's accent colour.</summary>
    public required string Accent { get; init; }

    /// <summary>Commits co-authored by Claude, when the project has any.</summary>
    public int? ClaudeCommits { get; init; }
    public required Text Problem { get; init; }
    public required Text Contribution { get; init; }
    public required IReadOnlyList<Highlight> Highlights { get; init; }
    public required IReadOnlyList<string> Stack { get; init; }
    public required IReadOnlyList<StatusBadge> Statuses { get; init; }
    public IReadOnlyList<Text> Implemented { get; init; } = [];
    public IReadOnlyList<Text> Roadmap { get; init; } = [];
    public IReadOnlyList<ProjectLink> Links { get; init; } = [];
    public Text? PrivateNote { get; init; }

    /// <summary>Name of the partial under Pages/Shared/Portfolio that draws the visual.</summary>
    public required string Visual { get; init; }
}

/// <summary>Secondary project rendered as a compact card.</summary>
public sealed class ProjectCard
{
    public required string Name { get; init; }
    public required Text Summary { get; init; }
    public required Text Category { get; init; }
    public required IReadOnlyList<string> Stack { get; init; }
    public required string RepoName { get; init; }

    /// <summary>Space-separated filter tags.</summary>
    public required string Tags { get; init; }
    public bool WithClaude { get; init; }
    public ProjectImage? Image { get; init; }
    public string? Glyph { get; init; }
    public ProjectLink? Live { get; init; }
    public StatusBadge? Status { get; init; }
}

public sealed record SkillGroup(Text Title, Text Caption, IReadOnlyList<string> Items);

public sealed record Milestone(string Period, Text Title, Text Body, IReadOnlyList<string> Tags);
