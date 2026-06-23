namespace HandballCompetitionManager.ViewModels;

public sealed class AdminUserDetailsViewModel
{
    public UserSummaryViewModel User { get; init; } = new();

    public TeamSummaryViewModel? AssignedTeam { get; init; }

    public IReadOnlyList<MatchSummaryViewModel> AssignedMatches { get; init; } = [];

    public string RoleLabel => string.Equals(User.Role, "Admin", StringComparison.OrdinalIgnoreCase)
        ? "Administrator"
        : User.Role;

    public int AssignedTeamsCount => AssignedTeam is null ? 0 : 1;

    public int UpcomingMatches => AssignedMatches.Count(match => string.Equals(match.Status, "Scheduled", StringComparison.OrdinalIgnoreCase));

    public int CompletedMatches => AssignedMatches.Count(match => string.Equals(match.Status, "Completed", StringComparison.OrdinalIgnoreCase));

    public string AccessLevel => string.Equals(User.Role, "Admin", StringComparison.OrdinalIgnoreCase)
        ? "Full system access"
        : "Role limited";

    public string ManagedTournamentsLabel => string.Equals(User.Role, "Admin", StringComparison.OrdinalIgnoreCase)
        ? "4"
        : "Not applicable";

    public bool IsCoach => string.Equals(User.Role, "Coach", StringComparison.OrdinalIgnoreCase);

    public bool IsReferee => string.Equals(User.Role, "Referee", StringComparison.OrdinalIgnoreCase);

    public bool IsAdministrator => string.Equals(User.Role, "Admin", StringComparison.OrdinalIgnoreCase);

    public bool IsActive => string.Equals(User.Status, "Active", StringComparison.OrdinalIgnoreCase);

    public bool EmailVerified => IsActive;

    public bool ProfileComplete => !string.IsNullOrWhiteSpace(User.FirstName)
        && !string.IsNullOrWhiteSpace(User.LastName)
        && !string.IsNullOrWhiteSpace(User.Email);
}
