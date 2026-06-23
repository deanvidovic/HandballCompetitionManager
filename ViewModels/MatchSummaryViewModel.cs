namespace HandballCompetitionManager.ViewModels;

public sealed class MatchSummaryViewModel
{
    public int Id { get; init; }

    public int TournamentId { get; init; }

    public int HomeTeamId { get; init; }

    public int AwayTeamId { get; init; }

    public string HomeTeamName { get; init; } = string.Empty;

    public string AwayTeamName { get; init; } = string.Empty;

    public string RoundName { get; init; } = string.Empty;

    public string VenueName { get; init; } = string.Empty;

    public string RefereeName { get; init; } = "Unassigned";

    public DateTime ScheduledAt { get; init; }

    public string Status { get; init; } = string.Empty;

    public int? HomeScore { get; init; }

    public int? AwayScore { get; init; }
}
