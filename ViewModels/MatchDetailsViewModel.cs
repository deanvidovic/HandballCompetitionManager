namespace HandballCompetitionManager.ViewModels;

public sealed class MatchDetailsViewModel
{
    public MatchSummaryViewModel Match { get; init; } = new();

    public TournamentCardViewModel Tournament { get; init; } = new();

    public TeamSummaryViewModel HomeTeam { get; init; } = new();

    public TeamSummaryViewModel AwayTeam { get; init; } = new();

    public MatchTotalsViewModel Totals { get; init; } = new();

    public TeamMatchComparisonViewModel HomeComparison { get; init; } = new();

    public TeamMatchComparisonViewModel AwayComparison { get; init; } = new();

    public IReadOnlyList<MatchEventSummaryViewModel> Events { get; init; } = [];

    public IReadOnlyList<MatchPlayerPerformanceViewModel> PlayerPerformances { get; init; } = [];

    public IReadOnlyList<MatchTopPerformerViewModel> TopPerformers { get; init; } = [];
}

public sealed class MatchTotalsViewModel
{
    public int TotalGoals { get; init; }

    public int YellowCards { get; init; }

    public int RedCards { get; init; }

    public int TwoMinuteSuspensions { get; init; }
}

public sealed class TeamMatchComparisonViewModel
{
    public int TeamId { get; init; }

    public string TeamName { get; init; } = string.Empty;

    public int Goals { get; init; }

    public int YellowCards { get; init; }

    public int RedCards { get; init; }

    public int TwoMinuteSuspensions { get; init; }
}

public sealed class MatchPlayerPerformanceViewModel
{
    public int PlayerId { get; init; }

    public int TeamId { get; init; }

    public string PlayerName { get; init; } = string.Empty;

    public string TeamName { get; init; } = string.Empty;

    public int Goals { get; init; }

    public int YellowCards { get; init; }

    public int RedCards { get; init; }

    public int TwoMinuteSuspensions { get; init; }

    public int DisciplinaryTotal => YellowCards + RedCards + TwoMinuteSuspensions;
}

public sealed class MatchTopPerformerViewModel
{
    public string Title { get; init; } = string.Empty;

    public int PlayerId { get; init; }

    public string PlayerName { get; init; } = string.Empty;

    public string TeamName { get; init; } = string.Empty;

    public string StatisticValue { get; init; } = string.Empty;
}
