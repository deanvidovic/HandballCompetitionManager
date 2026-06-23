using HandballCompetitionManager.Interfaces;
using HandballCompetitionManager.ViewModels;

namespace HandballCompetitionManager.Services.Mock;

public sealed class MockUserRepository : IUserRepository
{
    private static readonly IReadOnlyList<UserSummaryViewModel> Users =
    [
        new()
        {
            Id = 1,
            FullName = "Ana Horvat",
            FirstName = "Ana",
            LastName = "Horvat",
            Email = "ana.horvat@example.test",
            PhoneLabel = "+385 91 100 2001",
            Role = "Admin",
            Status = "Active",
            AssignedTeam = "System",
            AssignedTournament = "All tournaments",
            AssignedMatches = 0,
            CreatedDateLabel = "May 20, 2026",
            LastLoginLabel = "Today"
        },
        new()
        {
            Id = 2,
            FullName = "Ivan Kralj",
            FirstName = "Ivan",
            LastName = "Kralj",
            Email = "ivan.kralj@example.test",
            PhoneLabel = "+385 91 100 2002",
            Role = "Coach",
            Status = "Active",
            AssignedTeam = "RK Zagreb",
            AssignedTeamId = 1,
            AssignedTournament = "Croatia Handball Cup 2026",
            AssignedTournamentId = 1,
            AssignedTeamPlayerCount = 3,
            AssignedMatches = 0,
            CreatedDateLabel = "May 24, 2026",
            LastLoginLabel = "Yesterday"
        },
        new()
        {
            Id = 3,
            FullName = "Petra Maric",
            FirstName = "Petra",
            LastName = "Maric",
            Email = "petra.maric@example.test",
            PhoneLabel = "+385 91 100 2003",
            Role = "Coach",
            Status = "Active",
            AssignedTeam = "RK Split",
            AssignedTeamId = 3,
            AssignedTournament = "Croatia Handball Cup 2026",
            AssignedTournamentId = 1,
            AssignedTeamPlayerCount = 3,
            AssignedMatches = 0,
            CreatedDateLabel = "May 25, 2026",
            LastLoginLabel = "Jun 18, 2026"
        },
        new()
        {
            Id = 4,
            FullName = "Mario Blazevic",
            FirstName = "Mario",
            LastName = "Blazevic",
            Email = "mario.blazevic@example.test",
            PhoneLabel = "+385 91 100 2004",
            Role = "Referee",
            Status = "Active",
            AssignedTeam = "Unassigned",
            AssignedTournament = "Croatia Handball Cup 2026",
            AssignedTournamentId = 1,
            AssignedMatches = 4,
            CreatedDateLabel = "May 28, 2026",
            LastLoginLabel = "Today",
            City = "Zagreb",
            CertificationLevel = "National A",
            ProfileDescription = "National-level referee focused on fair play, clear communication, and consistent match management.",
            EmailVerified = true,
            ConfirmedReports = 2
        },
        new()
        {
            Id = 5,
            FullName = "Stipe Juric",
            FirstName = "Stipe",
            LastName = "Juric",
            Email = "stipe.juric@example.test",
            PhoneLabel = "+385 91 100 2005",
            Role = "Referee",
            Status = "Active",
            AssignedTeam = "Unassigned",
            AssignedTournament = "Croatia Handball Cup 2026",
            AssignedTournamentId = 1,
            AssignedMatches = 2,
            CreatedDateLabel = "May 29, 2026",
            LastLoginLabel = "Jun 20, 2026"
        },
        new()
        {
            Id = 6,
            FullName = "Dino Kovacevic",
            FirstName = "Dino",
            LastName = "Kovacevic",
            Email = "dino.kovacevic@example.test",
            PhoneLabel = "+385 91 100 2006",
            Role = "Referee",
            Status = "Inactive",
            AssignedTeam = "Unassigned",
            AssignedTournament = "Croatia Handball Cup 2026",
            AssignedTournamentId = 1,
            AssignedMatches = 1,
            CreatedDateLabel = "May 30, 2026",
            LastLoginLabel = "Jun 12, 2026"
        },
        new()
        {
            Id = 7,
            FullName = "Vedran Topic",
            FirstName = "Vedran",
            LastName = "Topic",
            Email = "vedran.topic@example.test",
            PhoneLabel = "+385 91 100 2007",
            Role = "Referee",
            Status = "Active",
            AssignedTeam = "Unassigned",
            AssignedTournament = "Croatia Handball Cup 2026",
            AssignedTournamentId = 1,
            AssignedMatches = 1,
            CreatedDateLabel = "Jun 1, 2026",
            LastLoginLabel = "Jun 19, 2026"
        }
    ];

    private static readonly IReadOnlyDictionary<int, UserSummaryViewModel> AssignedReferees = new Dictionary<int, UserSummaryViewModel>
    {
        [1] = Users[3],
        [2] = Users[4],
        [3] = Users[5],
        [4] = Users[3],
        [9] = Users[3],
        [17] = Users[3],
        [7] = Users[6],
        [8] = Users[4]
    };

    private static readonly IReadOnlyDictionary<int, IReadOnlyList<int>> AssignedMatchIdsByUserId = new Dictionary<int, IReadOnlyList<int>>
    {
        [4] = [1, 4, 17, 9],
        [5] = [2, 8],
        [6] = [3],
        [7] = [7]
    };

    public Task<IReadOnlyList<UserSummaryViewModel>> GetAllAsync()
    {
        return Task.FromResult(Users);
    }

    public Task<UserSummaryViewModel?> GetByIdAsync(int id)
    {
        return Task.FromResult(Users.FirstOrDefault(user => user.Id == id));
    }

    public Task<UserSummaryViewModel?> GetAssignedRefereeByMatchIdAsync(int matchId)
    {
        AssignedReferees.TryGetValue(matchId, out UserSummaryViewModel? referee);

        return Task.FromResult(referee);
    }

    public Task<IReadOnlyList<int>> GetAssignedMatchIdsByUserIdAsync(int userId)
    {
        AssignedMatchIdsByUserId.TryGetValue(userId, out IReadOnlyList<int>? matchIds);

        return Task.FromResult(matchIds ?? []);
    }

}
