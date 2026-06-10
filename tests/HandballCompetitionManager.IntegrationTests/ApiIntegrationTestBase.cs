using HandballCompetitionManager.Data;
using HandballCompetitionManager.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HandballCompetitionManager.IntegrationTests;

public abstract class ApiIntegrationTestBase : IAsyncLifetime
{
    protected readonly CustomWebApplicationFactory Factory = new();
    protected readonly HttpClient Client;

    protected ApiIntegrationTestBase()
    {
        Client = Factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<HandballDbContext>();

        db.Matches.RemoveRange(db.Matches);
        db.GroupPhases.RemoveRange(db.GroupPhases);
        db.Players.RemoveRange(db.Players);
        db.Teams.RemoveRange(db.Teams);
        db.Competitions.RemoveRange(db.Competitions);
        db.AppUsers.RemoveRange(db.AppUsers);
        await db.SaveChangesAsync();

        var admin = new AppUser
        {
            UserName = "admin",
            DisplayName = "Admin User",
            Email = "admin@test.local",
            EmailConfirmed = true,
            Role = UserRole.Admin,
            OIB = "12345678901",
            JMBG = "1234567890123",
            CreatedAt = DateTime.UtcNow
        };

        var manager = new AppUser
        {
            UserName = "manager",
            DisplayName = "Manager User",
            Email = "manager@test.local",
            EmailConfirmed = true,
            Role = UserRole.TournamentManager,
            OIB = "12345678902",
            JMBG = "1234567890124",
            CreatedAt = DateTime.UtcNow
        };

        var homeTeam = new Team
        {
            Name = "RK Test Home",
            CoachName = "Coach Home",
            HomeCity = "Zagreb",
            FoundedYear = 2001,
            HomeArena = "Home Arena"
        };

        var awayTeam = new Team
        {
            Name = "RK Test Away",
            CoachName = "Coach Away",
            HomeCity = "Split",
            FoundedYear = 2002,
            HomeArena = "Away Arena"
        };

        var competition = new Competition
        {
            Name = "Test Cup",
            Season = "2025/2026",
            City = "Zagreb",
            StartDate = new DateTime(2026, 6, 1),
            EndDate = new DateTime(2026, 6, 10),
            Teams = new List<Team> { homeTeam, awayTeam },
            Administrators = new List<AppUser> { admin, manager }
        };

        var group = new GroupPhase
        {
            Name = "Group A",
            Competition = competition,
            Teams = new List<Team> { homeTeam, awayTeam }
        };

        var player = new Player
        {
            FirstName = "Test",
            LastName = "Player",
            BirthDate = new DateTime(2000, 1, 1),
            JerseyNumber = 9,
            Position = PlayerPosition.CenterBack,
            Team = homeTeam,
            GoalsScored = 5,
            Assists = 2
        };

        var match = new Match
        {
            Competition = competition,
            GroupPhase = group,
            RoundNumber = 1,
            Kickoff = new DateTime(2026, 6, 2, 18, 0, 0),
            HomeTeam = homeTeam,
            AwayTeam = awayTeam,
            HomeScore = 20,
            AwayScore = 18,
            MaintenanceHall = "Home Arena",
            Status = MatchStatus.Finished
        };

        db.AppUsers.AddRange(admin, manager);
        db.Teams.AddRange(homeTeam, awayTeam);
        db.Competitions.Add(competition);
        db.GroupPhases.Add(group);
        db.Players.Add(player);
        db.Matches.Add(match);
        await db.SaveChangesAsync();
    }

    public Task DisposeAsync()
    {
        Client.Dispose();
        Factory.Dispose();
        return Task.CompletedTask;
    }

    protected async Task<T> WithDb<T>(Func<HandballDbContext, Task<T>> action)
    {
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<HandballDbContext>();
        return await action(db);
    }
}
