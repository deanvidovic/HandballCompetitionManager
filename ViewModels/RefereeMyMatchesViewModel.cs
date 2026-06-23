namespace HandballCompetitionManager.ViewModels;

public sealed class RefereeMyMatchesViewModel
{
    public UserSummaryViewModel? Referee { get; init; }

    public IReadOnlyList<RefereeMatchRowViewModel> Matches { get; init; } = [];

    public int AssignedMatches => Matches.Count;

    public int TodayMatches => Matches.Count(match => match.ScheduledAt.Date == DateTime.Today);

    public int UpcomingMatches => Matches.Count(match =>
        !string.Equals(match.Status, "Completed", StringComparison.OrdinalIgnoreCase)
        && match.ScheduledAt.Date > DateTime.Today);

    public int CompletedMatches => Matches.Count(match =>
        string.Equals(match.Status, "Completed", StringComparison.OrdinalIgnoreCase));

    public IReadOnlyList<RefereeMatchRowViewModel> Today => Matches
        .Where(match => match.ScheduledAt.Date == DateTime.Today)
        .OrderBy(match => match.ScheduledAt)
        .ToList();

    public IReadOnlyList<RefereeMatchRowViewModel> Upcoming => Matches
        .Where(match =>
            !string.Equals(match.Status, "Completed", StringComparison.OrdinalIgnoreCase)
            && match.ScheduledAt.Date > DateTime.Today)
        .OrderBy(match => match.ScheduledAt)
        .ToList();

    public IReadOnlyList<RefereeMatchRowViewModel> Completed => Matches
        .Where(match => string.Equals(match.Status, "Completed", StringComparison.OrdinalIgnoreCase))
        .OrderByDescending(match => match.ScheduledAt)
        .ToList();
}

public sealed class RefereeMatchRowViewModel
{
    public int Id { get; init; }

    public string HomeTeamName { get; init; } = string.Empty;

    public string AwayTeamName { get; init; } = string.Empty;

    public string TournamentName { get; init; } = string.Empty;

    public string Phase { get; init; } = string.Empty;

    public DateTime ScheduledAt { get; init; }

    public string Status { get; init; } = string.Empty;

    public string VenueName { get; init; } = string.Empty;

    public string Score { get; init; } = string.Empty;
}
