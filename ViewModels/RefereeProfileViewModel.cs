namespace HandballCompetitionManager.ViewModels;

public sealed class RefereeProfileViewModel
{
    public UserSummaryViewModel Referee { get; init; } = new();

    public int AssignedMatches { get; init; }

    public int UpcomingMatches { get; init; }

    public int CompletedMatches { get; init; }

    public int ConfirmedReports { get; init; }
}
