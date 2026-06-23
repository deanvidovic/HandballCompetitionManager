namespace HandballCompetitionManager.ViewModels;

public sealed class AdminPlayerManagementViewModel
{
    public IReadOnlyList<AdminPlayerRowViewModel> Players { get; init; } = [];

    public int TotalPlayers => Players.Count;

    public int ActivePlayers => Players.Count;

    public int RegisteredTeams => Players
        .Where(player => !string.IsNullOrWhiteSpace(player.TeamName))
        .Select(player => player.TeamName)
        .Distinct()
        .Count();

    public int TotalGoals => Players.Sum(player => player.Goals);
}

public sealed class AdminPlayerRowViewModel
{
    public int Id { get; init; }

    public string FullName { get; init; } = string.Empty;

    public string TeamName { get; init; } = string.Empty;

    public string Position { get; init; } = string.Empty;

    public int ShirtNumber { get; init; }

    public int Goals { get; init; }

    public int YellowCards { get; init; }

    public int RedCards { get; init; }

    public int TwoMinuteSuspensions { get; init; }
}
