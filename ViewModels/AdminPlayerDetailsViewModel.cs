namespace HandballCompetitionManager.ViewModels;

public sealed class AdminPlayerDetailsViewModel
{
    public PlayerSummaryViewModel Player { get; init; } = new();

    public TeamSummaryViewModel Team { get; init; } = new();

    public TournamentCardViewModel Tournament { get; init; } = new();

    public PlayerStatisticViewModel Statistics { get; init; } = new();

    public TeamPerformanceViewModel TeamPerformance { get; init; } = new();

    public IReadOnlyList<AdminPlayerRecentMatchViewModel> RecentMatches { get; init; } = [];

    public IReadOnlyList<AdminPlayerPerformanceCardViewModel> PerformanceCards { get; init; } = [];

    public int Age { get; init; }

    public string Status { get; init; } = "Active";

    public int MatchesPlayed => RecentMatches.Count;

    public int Assists { get; init; }

    public decimal AverageGoalsPerMatch => MatchesPlayed == 0 ? 0 : Math.Round((decimal)Statistics.Goals / MatchesPlayed, 1);
}

public sealed class AdminPlayerRecentMatchViewModel
{
    public int MatchId { get; init; }

    public DateTime MatchDate { get; init; }

    public string OpponentName { get; init; } = string.Empty;

    public string Phase { get; init; } = string.Empty;

    public string Result { get; init; } = string.Empty;

    public int Goals { get; init; }

    public int YellowCards { get; init; }

    public int RedCards { get; init; }

    public int TwoMinuteSuspensions { get; init; }

    public string CardsSummary
    {
        get
        {
            IReadOnlyList<string> cards =
            [
                YellowCards > 0 ? $"{YellowCards} YC" : string.Empty,
                RedCards > 0 ? $"{RedCards} RC" : string.Empty,
                TwoMinuteSuspensions > 0 ? $"{TwoMinuteSuspensions} 2M" : string.Empty
            ];

            string summary = string.Join(", ", cards.Where(card => !string.IsNullOrWhiteSpace(card)));
            return string.IsNullOrWhiteSpace(summary) ? "None" : summary;
        }
    }
}

public sealed class AdminPlayerPerformanceCardViewModel
{
    public string Label { get; init; } = string.Empty;

    public string Value { get; init; } = string.Empty;

    public string Detail { get; init; } = string.Empty;
}
