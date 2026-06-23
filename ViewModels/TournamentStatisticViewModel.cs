namespace HandballCompetitionManager.ViewModels;

public sealed class TournamentStatisticViewModel
{
    public int TournamentId { get; init; }

    public int TotalTeams { get; init; }

    public int TotalPlayers { get; init; }

    public int TotalMatches { get; init; }

    public int CompletedMatches { get; init; }

    public int TotalGoals { get; init; }
}
