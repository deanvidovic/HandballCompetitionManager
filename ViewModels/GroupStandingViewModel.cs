namespace HandballCompetitionManager.ViewModels;

public sealed class GroupStandingViewModel
{
    public int TournamentId { get; init; }

    public int TeamId { get; init; }

    public string GroupName { get; init; } = string.Empty;

    public string TeamName { get; init; } = string.Empty;

    public int Played { get; init; }

    public int Won { get; init; }

    public int Drawn { get; init; }

    public int Lost { get; init; }

    public int GoalsFor { get; init; }

    public int GoalsAgainst { get; init; }

    public int Points { get; init; }
}
