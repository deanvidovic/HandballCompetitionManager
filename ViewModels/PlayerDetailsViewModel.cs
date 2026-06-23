namespace HandballCompetitionManager.ViewModels;

public sealed class PlayerDetailsViewModel
{
    public PlayerSummaryViewModel Player { get; init; } = new();

    public TeamSummaryViewModel Team { get; init; } = new();

    public TournamentCardViewModel Tournament { get; init; } = new();

    public PlayerStatisticViewModel Statistics { get; init; } = new();

    public PlayerTournamentPerformanceViewModel Performance { get; init; } = new();

    public TeamPerformanceViewModel TeamPerformance { get; init; } = new();

    public string Description { get; init; } = string.Empty;

    public string AgeDisplay { get; init; } = string.Empty;

    public string TournamentPosition { get; init; } = string.Empty;

    public IReadOnlyList<PlayerMatchPerformanceViewModel> MatchPerformances { get; init; } = [];

    public IReadOnlyList<PlayerRankingViewModel> Rankings { get; init; } = [];
}

public sealed class PlayerTournamentPerformanceViewModel
{
    public int Goals { get; init; }

    public int Assists { get; init; }

    public int MatchesPlayed { get; init; }

    public int YellowCards { get; init; }

    public int RedCards { get; init; }

    public int TwoMinuteSuspensions { get; init; }

    public decimal AverageGoalsPerMatch => MatchesPlayed == 0 ? 0 : Math.Round((decimal)Goals / MatchesPlayed, 1);
}

public sealed class PlayerMatchPerformanceViewModel
{
    public int MatchId { get; init; }

    public DateTime MatchDate { get; init; }

    public string OpponentName { get; init; } = string.Empty;

    public int Goals { get; init; }

    public int YellowCards { get; init; }

    public int RedCards { get; init; }

    public int TwoMinuteSuspensions { get; init; }
}

public sealed class PlayerRankingViewModel
{
    public string Label { get; init; } = string.Empty;

    public int Rank { get; init; }

    public int Value { get; init; }
}
