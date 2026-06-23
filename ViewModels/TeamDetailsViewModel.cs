namespace HandballCompetitionManager.ViewModels;

public sealed class TeamDetailsViewModel
{
    public TeamSummaryViewModel Team { get; init; } = new();

    public TournamentCardViewModel Tournament { get; init; } = new();

    public TeamPerformanceViewModel Performance { get; init; } = new();

    public string Description { get; init; } = string.Empty;

    public string GroupName { get; init; } = string.Empty;

    public IReadOnlyList<TeamRosterPlayerViewModel> Roster { get; init; } = [];

    public IReadOnlyList<MatchSummaryViewModel> Matches { get; init; } = [];

    public IReadOnlyList<StatisticLeaderboardViewModel> PlayerLeaderboards { get; init; } = [];
}

public sealed class TeamPerformanceViewModel
{
    public int MatchesPlayed { get; init; }

    public int Wins { get; init; }

    public int Draws { get; init; }

    public int Losses { get; init; }

    public int GoalsScored { get; init; }

    public int GoalsConceded { get; init; }

    public int GoalDifference => GoalsScored - GoalsConceded;

    public string Record => $"{Wins}-{Draws}-{Losses}";
}

public sealed class TeamRosterPlayerViewModel
{
    public int Id { get; init; }

    public string FullName { get; init; } = string.Empty;

    public int ShirtNumber { get; init; }

    public string Position { get; init; } = string.Empty;

    public int Goals { get; init; }

    public int YellowCards { get; init; }

    public int RedCards { get; init; }

    public int TwoMinuteSuspensions { get; init; }
}
