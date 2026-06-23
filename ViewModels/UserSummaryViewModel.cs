namespace HandballCompetitionManager.ViewModels;

public sealed class UserSummaryViewModel
{
    public int Id { get; init; }

    public string FullName { get; init; } = string.Empty;

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string PhoneLabel { get; init; } = string.Empty;

    public string Role { get; init; } = string.Empty;

    public string Status { get; init; } = string.Empty;

    public string AssignedTeam { get; init; } = string.Empty;

    public int? AssignedTeamId { get; init; }

    public string AssignedTournament { get; init; } = string.Empty;

    public int? AssignedTournamentId { get; init; }

    public int AssignedTeamPlayerCount { get; init; }

    public int AssignedMatches { get; init; }

    public string CreatedDateLabel { get; init; } = string.Empty;

    public string LastLoginLabel { get; init; } = string.Empty;

    public string City { get; init; } = string.Empty;

    public string CertificationLevel { get; init; } = string.Empty;

    public string ProfileDescription { get; init; } = string.Empty;

    public bool EmailVerified { get; init; }

    public int ConfirmedReports { get; init; }

}
