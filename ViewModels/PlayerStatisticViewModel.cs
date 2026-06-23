namespace HandballCompetitionManager.ViewModels;

public sealed class PlayerStatisticViewModel
{
    public int TournamentId { get; init; }

    public int PlayerId { get; init; }

    public int TeamId { get; init; }

    public string PlayerName { get; init; } = string.Empty;

    public string TeamName { get; init; } = string.Empty;

    public int Goals { get; init; }

    public int YellowCards { get; init; }

    public int RedCards { get; init; }

    public int TwoMinuteSuspensions { get; init; }
}
