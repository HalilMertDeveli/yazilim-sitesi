namespace Portfolio.Models;

public sealed class LanguageStat
{
    public required string Name { get; init; }
    public int Count { get; init; }
    public int Percent { get; init; }
}
