namespace HandballCompetitionManager.ViewModels;

public sealed class AdminTeamManagementViewModel
{
    public IReadOnlyList<AdminTeamRowViewModel> Teams { get; init; } = [];

    public int TotalTeams => Teams.Count;

    public int ActiveTeams => Teams.Count;

    public int TotalPlayers => Teams.Sum(team => team.PlayerCount);

    public int TeamsInTournaments => Teams.Count(team => !string.IsNullOrWhiteSpace(team.TournamentName));
}

public sealed class AdminTeamRowViewModel
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string City { get; init; } = string.Empty;

    public string CoachName { get; init; } = string.Empty;

    public int PlayerCount { get; init; }

    public string TournamentName { get; init; } = string.Empty;

    public string Record { get; init; } = string.Empty;

    public string Goals { get; init; } = string.Empty;
}
