namespace HandballCompetitionManager.ViewModels;

public sealed class AdminTeamDetailsViewModel
{
    public TeamSummaryViewModel Team { get; init; } = new();

    public TournamentCardViewModel Tournament { get; init; } = new();

    public TeamPerformanceViewModel Performance { get; init; } = new();

    public IReadOnlyList<AdminTeamPlayerRowViewModel> Players { get; init; } = [];

    public IReadOnlyList<AdminTeamMatchRowViewModel> Matches { get; init; } = [];

    public IReadOnlyList<AdminTeamStatisticCardViewModel> StatisticCards { get; init; } = [];

    public int PlayerCount => Players.Count;

    public bool CoachAssigned => !string.IsNullOrWhiteSpace(Team.CoachName);

    public bool PlayersRegistered => Players.Count > 0;

    public bool TournamentAssigned => Tournament.Id > 0;

    public bool TeamReady => CoachAssigned && PlayersRegistered && TournamentAssigned;
}

public sealed class AdminTeamPlayerRowViewModel
{
    public int Id { get; init; }

    public int ShirtNumber { get; init; }

    public string FullName { get; init; } = string.Empty;

    public string Position { get; init; } = string.Empty;

    public int Age { get; init; }

    public int Goals { get; init; }

    public int YellowCards { get; init; }

    public int RedCards { get; init; }

    public int TwoMinuteSuspensions { get; init; }
}

public sealed class AdminTeamMatchRowViewModel
{
    public int Id { get; init; }

    public string OpponentName { get; init; } = string.Empty;

    public string Phase { get; init; } = string.Empty;

    public DateTime ScheduledAt { get; init; }

    public string Result { get; init; } = string.Empty;

    public string Status { get; init; } = string.Empty;
}

public sealed class AdminTeamStatisticCardViewModel
{
    public string Title { get; init; } = string.Empty;

    public int PlayerId { get; init; }

    public string PlayerName { get; init; } = string.Empty;

    public string ValueLabel { get; init; } = string.Empty;
}
