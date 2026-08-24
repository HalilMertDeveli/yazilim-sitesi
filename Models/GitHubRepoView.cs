namespace Portfolio.Models;

public sealed class GitHubRepoView
{
    public required string Name { get; init; }
    public required string Url { get; init; }
    public string? Description { get; init; }
    public string? Language { get; init; }
    public int Stars { get; init; }
    public string? UpdatedAt { get; init; }
}
