namespace HandballCompetitionManager.ViewModels;

public sealed class AdminMatchManagementViewModel
{
    public IReadOnlyList<AdminMatchRowViewModel> Matches { get; init; } = [];

    public int TotalMatches => Matches.Count;

    public int UpcomingMatches => Matches.Count(match => string.Equals(match.Status, "Scheduled", StringComparison.OrdinalIgnoreCase));

    public int InProgressMatches => Matches.Count(match => string.Equals(match.Status, "In Progress", StringComparison.OrdinalIgnoreCase));

    public int CompletedMatches => Matches.Count(match => string.Equals(match.Status, "Completed", StringComparison.OrdinalIgnoreCase));

    public int UpcomingToday => Matches.Count(match =>
        string.Equals(match.Status, "Scheduled", StringComparison.OrdinalIgnoreCase)
        && match.ScheduledAt.Date == DateTime.Today);

    public int UpcomingThisWeek => Matches.Count(match =>
        string.Equals(match.Status, "Scheduled", StringComparison.OrdinalIgnoreCase)
        && match.ScheduledAt.Date >= DateTime.Today
        && match.ScheduledAt.Date <= DateTime.Today.AddDays(7));

    public int MatchesWithoutAssignedReferee => Matches.Count(match =>
        string.IsNullOrWhiteSpace(match.RefereeName)
        || string.Equals(match.RefereeName, "Unassigned", StringComparison.OrdinalIgnoreCase));

    public int CompletedAwaitingConfirmation => 0;
}

public sealed class AdminMatchRowViewModel
{
    public int Id { get; init; }

    public string HomeTeamName { get; init; } = string.Empty;

    public string AwayTeamName { get; init; } = string.Empty;

    public string TournamentName { get; init; } = string.Empty;

    public string Phase { get; init; } = string.Empty;

    public string VenueName { get; init; } = string.Empty;

    public string RefereeName { get; init; } = string.Empty;

    public DateTime ScheduledAt { get; init; }

    public string Status { get; init; } = string.Empty;

    public string Score { get; init; } = string.Empty;

    public bool IsCompleted => string.Equals(Status, "Completed", StringComparison.OrdinalIgnoreCase);
}
