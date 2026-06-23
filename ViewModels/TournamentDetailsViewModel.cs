namespace HandballCompetitionManager.ViewModels;

public sealed class TournamentDetailsViewModel
{
    public TournamentCardViewModel Tournament { get; init; } = new();

    public TournamentStatisticViewModel Statistics { get; init; } = new();

    public IReadOnlyList<TournamentTeamDetailsViewModel> Teams { get; init; } = [];

    public IReadOnlyList<TournamentGroupViewModel> Groups { get; init; } = [];

    public IReadOnlyList<TournamentBracketRoundViewModel> BracketRounds { get; init; } = [];

    public IReadOnlyList<MatchSummaryViewModel> GroupMatches { get; init; } = [];

    public IReadOnlyList<MatchSummaryViewModel> EliminationMatches { get; init; } = [];

    public IReadOnlyList<MatchSummaryViewModel> RecentMatches { get; init; } = [];

    public IReadOnlyList<StatisticLeaderboardViewModel> PlayerLeaderboards { get; init; } = [];
}

public sealed class TournamentTeamDetailsViewModel
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string City { get; init; } = string.Empty;

    public string CoachName { get; init; } = string.Empty;

    public int PlayerCount { get; init; }

    public string Record { get; init; } = string.Empty;

    public int GoalsScored { get; init; }

    public int GoalsConceded { get; init; }
}

public sealed class TournamentGroupViewModel
{
    public string Name { get; init; } = string.Empty;

    public IReadOnlyList<GroupStandingRowViewModel> Standings { get; init; } = [];
}

public sealed class GroupStandingRowViewModel
{
    public int Position { get; init; }

    public GroupStandingViewModel Standing { get; init; } = new();

    public int GoalDifference => Standing.GoalsFor - Standing.GoalsAgainst;
}

public sealed class TournamentBracketRoundViewModel
{
    public string Name { get; init; } = string.Empty;

    public IReadOnlyList<MatchSummaryViewModel> Matches { get; init; } = [];
}

public sealed class StatisticLeaderboardViewModel
{
    public string Title { get; init; } = string.Empty;

    public string StatLabel { get; init; } = string.Empty;

    public IReadOnlyList<StatisticLeaderViewModel> Leaders { get; init; } = [];
}

public sealed class StatisticLeaderViewModel
{
    public int Rank { get; init; }

    public int PlayerId { get; init; }

    public string PlayerName { get; init; } = string.Empty;

    public string TeamName { get; init; } = string.Empty;

    public int Value { get; init; }
}
