namespace HandballCompetitionManager.ViewModels;

public sealed class AdminTournamentManagementViewModel
{
    public required IReadOnlyList<TournamentCardViewModel> Tournaments { get; init; }

    public int TotalTournaments => Tournaments.Count;

    public int ActiveTournaments => CountByStatus("Active");

    public int UpcomingTournaments => CountByStatus("Upcoming");

    public int CompletedTournaments => CountByStatus("Completed");

    private int CountByStatus(string status)
    {
        return Tournaments.Count(tournament => string.Equals(tournament.Status, status, StringComparison.OrdinalIgnoreCase));
    }
}
