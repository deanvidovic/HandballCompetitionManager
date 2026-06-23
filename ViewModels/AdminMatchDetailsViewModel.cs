namespace HandballCompetitionManager.ViewModels;

public sealed class AdminMatchDetailsViewModel
{
    public MatchSummaryViewModel Match { get; init; } = new();

    public TournamentCardViewModel Tournament { get; init; } = new();

    public TeamSummaryViewModel HomeTeam { get; init; } = new();

    public TeamSummaryViewModel AwayTeam { get; init; } = new();

    public UserSummaryViewModel? Referee { get; init; }

    public MatchTotalsViewModel Totals { get; init; } = new();

    public TeamMatchComparisonViewModel HomeComparison { get; init; } = new();

    public TeamMatchComparisonViewModel AwayComparison { get; init; } = new();

    public IReadOnlyList<MatchEventSummaryViewModel> Events { get; init; } = [];

    public IReadOnlyList<MatchPlayerPerformanceViewModel> PlayerPerformances { get; init; } = [];

    public string AssignedRefereeName => Referee?.FullName ?? Match.RefereeName;

    public string Score => Match.HomeScore.HasValue && Match.AwayScore.HasValue
        ? $"{Match.HomeScore} - {Match.AwayScore}"
        : "Not played";

    public string AttendanceLabel => Match.Status == "Completed" ? "1,240" : "Pending";

    public string DurationLabel => Match.Status == "Completed" ? "60 min" : "Pending";

    public bool HasAssignedReferee => Referee is not null || !string.Equals(Match.RefereeName, "Unassigned", StringComparison.OrdinalIgnoreCase);

    public bool TeamSheetsSubmitted => Match.Status == "Completed" || Match.Status == "In Progress";

    public bool MatchConfirmed => Match.Status == "Completed";

    public bool StatisticsComplete => Match.Status == "Completed" && PlayerPerformances.Count > 0;
}
