namespace HandballCompetitionManager.ViewModels;

public sealed class MatchEventSummaryViewModel
{
    public int Id { get; init; }

    public int MatchId { get; init; }

    public int PlayerId { get; init; }

    public int TeamId { get; init; }

    public string PlayerName { get; init; } = string.Empty;

    public string TeamName { get; init; } = string.Empty;

    public string EventType { get; init; } = string.Empty;

    public int Minute { get; init; }
}
