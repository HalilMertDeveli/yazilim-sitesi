namespace Portfolio.Models;

public sealed record GitHubRepoInfo(
    string Name,
    string Url,
    string? Description,
    string? Language,
    int Stars,
    DateTimeOffset? PushedAt);

public sealed record LanguageShare(string Name, int Count, int Percent);

/// <summary>Public GitHub data shown next to the curated content. Always renderable.</summary>
public sealed class GitHubSnapshot
{
    public required int PublicRepos { get; init; }
    public required IReadOnlyList<LanguageShare> Languages { get; init; }
    public required IReadOnlyList<GitHubRepoInfo> RecentRepos { get; init; }
    public required IReadOnlyDictionary<string, GitHubRepoInfo> ByName { get; init; }
    public DateTimeOffset? LastPush { get; init; }
    public bool IsLive { get; init; }

    public GitHubRepoInfo? Find(string? name) =>
        name is not null && ByName.TryGetValue(name, out var repo) ? repo : null;
}
