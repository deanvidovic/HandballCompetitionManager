namespace HandballCompetitionManager.ViewModels;

public sealed class TournamentCardViewModel
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Status { get; init; } = string.Empty;

    public string Location { get; init; } = string.Empty;

    public string StartDate { get; init; } = string.Empty;

    public string EndDate { get; init; } = string.Empty;

    public int TeamCount { get; init; }

    public int MatchCount { get; init; }

    public string CurrentPhase { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;
}
