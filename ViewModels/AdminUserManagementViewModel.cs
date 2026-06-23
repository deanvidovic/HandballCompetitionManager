namespace HandballCompetitionManager.ViewModels;

public sealed class AdminUserManagementViewModel
{
    public IReadOnlyList<UserSummaryViewModel> Users { get; init; } = [];

    public int TotalUsers => Users.Count;

    public int Administrators => CountRole("Admin");

    public int Coaches => CountRole("Coach");

    public int Referees => CountRole("Referee");

    private int CountRole(string role)
    {
        return Users.Count(user => string.Equals(user.Role, role, StringComparison.OrdinalIgnoreCase));
    }
}
