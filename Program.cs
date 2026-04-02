using HandballCompetitionManager.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

var labData = BuildLabData();
RunLabQueries(labData);
await DemonstrateAsyncAwait();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

static LabData BuildLabData()
{
    var users = new List<AppUser>
    {
        new()
        {
            Id = 1,
            Username = "admin",
            DisplayName = "System Admin",
            Email = "admin@hcm.local",
            Role = UserRole.Admin,
            CreatedAt = new DateTime(2026, 1, 10)
        },
        new()
        {
            Id = 2,
            Username = "tm.zagreb",
            DisplayName = "Zagreb Manager",
            Email = "tm.zagreb@hcm.local",
            Role = UserRole.TournamentManager,
            CreatedAt = new DateTime(2026, 1, 15),
            ManagedCompetitionIds = new List<int> { 1, 2 }
        },
        new()
        {
            Id = 3,
            Username = "coach.metalac",
            DisplayName = "Coach Metalac",
            Email = "coach.metalac@hcm.local",
            Role = UserRole.Coach,
            CreatedAt = new DateTime(2026, 2, 1),
            ManagedCompetitionIds = new List<int> { 1 }
        },
        new()
        {
            Id = 4,
            Username = "guest.one",
            DisplayName = "Guest User",
            Email = "guest@hcm.local",
            Role = UserRole.Guest,
            CreatedAt = new DateTime(2026, 2, 5)
        }
    };

    var teams = new List<Team>
    {
        new()
        {
            Id = 1,
            Name = "RK Metalac",
            Club = "Metalac",
            CoachName = "Ivan Horvat",
            HomeCity = "Zagreb",
            FoundedYear = 1965,
            HomeArena = "Arena Trešnjevka"
        },
        new()
        {
            Id = 2,
            Name = "RK Medvescak",
            Club = "Medvescak",
            CoachName = "Luka Gajic",
            HomeCity = "Zagreb",
            FoundedYear = 1973,
            HomeArena = "Dom Sportova"
        },
        new()
        {
            Id = 3,
            Name = "RK Sisak",
            Club = "Sisak",
            CoachName = "Petar Radic",
            HomeCity = "Sisak",
            FoundedYear = 1980,
            HomeArena = "SD Sisak"
        },
        new()
        {
            Id = 4,
            Name = "RK Vukovar",
            Club = "Vukovar",
            CoachName = "Marko Kolar",
            HomeCity = "Vukovar",
            FoundedYear = 1979,
            HomeArena = "Borovo Hall"
        },
        new()
        {
            Id = 5,
            Name = "RK Split",
            Club = "Split",
            CoachName = "Josip Marin",
            HomeCity = "Split",
            FoundedYear = 1958,
            HomeArena = "Gripe"
        },
        new()
        {
            Id = 6,
            Name = "RK Zadar",
            Club = "Zadar",
            CoachName = "Ante Klaric",
            HomeCity = "Zadar",
            FoundedYear = 1969,
            HomeArena = "Jazine"
        },
        new()
        {
            Id = 7,
            Name = "RK Osijek",
            Club = "Osijek",
            CoachName = "Dario Bulat",
            HomeCity = "Osijek",
            FoundedYear = 1970,
            HomeArena = "Gradski vrt"
        },
        new()
        {
            Id = 8,
            Name = "RK Karlovac",
            Club = "Karlovac",
            CoachName = "Tomislav Knez",
            HomeCity = "Karlovac",
            FoundedYear = 1975,
            HomeArena = "Mladost"
        },
        new()
        {
            Id = 9,
            Name = "RK Porec",
            Club = "Porec",
            CoachName = "Nino Peric",
            HomeCity = "Porec",
            FoundedYear = 1984,
            HomeArena = "Veli Joze"
        }
    };

    var players = new List<Player>
    {
        new() { Id = 1, FirstName = "Filip", LastName = "Grgic", BirthDate = new DateTime(1999, 5, 12), JerseyNumber = 1, Position = PlayerPosition.Goalkeeper, TeamId = 1, GoalsScored = 0, Assists = 1 },
        new() { Id = 2, FirstName = "Mario", LastName = "Celic", BirthDate = new DateTime(2001, 7, 1), JerseyNumber = 9, Position = PlayerPosition.LeftBack, TeamId = 1, GoalsScored = 33, Assists = 12 },
        new() { Id = 3, FirstName = "Ivan", LastName = "Basic", BirthDate = new DateTime(2000, 3, 14), JerseyNumber = 11, Position = PlayerPosition.RightWing, TeamId = 1, GoalsScored = 26, Assists = 18 },
        new() { Id = 4, FirstName = "Luka", LastName = "Pavic", BirthDate = new DateTime(1998, 2, 24), JerseyNumber = 8, Position = PlayerPosition.CenterBack, TeamId = 2, GoalsScored = 21, Assists = 30 },
        new() { Id = 5, FirstName = "Karlo", LastName = "Nikic", BirthDate = new DateTime(2002, 11, 9), JerseyNumber = 14, Position = PlayerPosition.Pivot, TeamId = 2, GoalsScored = 24, Assists = 8 },
        new() { Id = 6, FirstName = "Tin", LastName = "Matic", BirthDate = new DateTime(2003, 9, 17), JerseyNumber = 16, Position = PlayerPosition.Goalkeeper, TeamId = 2, GoalsScored = 0, Assists = 0 },
        new() { Id = 7, FirstName = "Noa", LastName = "Jurisic", BirthDate = new DateTime(2000, 1, 18), JerseyNumber = 10, Position = PlayerPosition.LeftWing, TeamId = 3, GoalsScored = 22, Assists = 9 },
        new() { Id = 8, FirstName = "David", LastName = "Bosanac", BirthDate = new DateTime(1999, 12, 30), JerseyNumber = 5, Position = PlayerPosition.RightBack, TeamId = 3, GoalsScored = 17, Assists = 14 },
        new() { Id = 9, FirstName = "Jakov", LastName = "Mikic", BirthDate = new DateTime(2001, 4, 28), JerseyNumber = 7, Position = PlayerPosition.CenterBack, TeamId = 3, GoalsScored = 18, Assists = 20 },
        new() { Id = 10, FirstName = "Leon", LastName = "Sesar", BirthDate = new DateTime(1998, 8, 19), JerseyNumber = 3, Position = PlayerPosition.LeftBack, TeamId = 4, GoalsScored = 31, Assists = 10 },
        new() { Id = 11, FirstName = "Matej", LastName = "Jelavic", BirthDate = new DateTime(1997, 6, 3), JerseyNumber = 6, Position = PlayerPosition.Pivot, TeamId = 5, GoalsScored = 28, Assists = 5 },
        new() { Id = 12, FirstName = "Roko", LastName = "Vidic", BirthDate = new DateTime(2004, 10, 22), JerseyNumber = 2, Position = PlayerPosition.RightWing, TeamId = 6, GoalsScored = 20, Assists = 11 }
    };

    foreach (var team in teams)
    {
        team.Players = players.Where(p => p.TeamId == team.Id).ToList();
    }

    var competitions = new List<Competition>
    {
        new()
        {
            Id = 1,
            Name = "Zagreb Spring Cup",
            Season = "2025/26",
            StartDate = new DateTime(2026, 4, 10),
            EndDate = new DateTime(2026, 4, 25),
            City = "Zagreb",
            Teams = teams.Where(t => t.Id is 1 or 2 or 3).ToList(),
            Administrators = users.Where(u => u.Id is 1 or 2).ToList()
        },
        new()
        {
            Id = 2,
            Name = "Adriatic Handball Trophy",
            Season = "2025/26",
            StartDate = new DateTime(2026, 5, 3),
            EndDate = new DateTime(2026, 5, 20),
            City = "Split",
            Teams = teams.Where(t => t.Id is 4 or 5 or 6).ToList(),
            Administrators = users.Where(u => u.Id is 1 or 2).ToList()
        },
        new()
        {
            Id = 3,
            Name = "Panonia Challenge",
            Season = "2025/26",
            StartDate = new DateTime(2026, 6, 1),
            EndDate = new DateTime(2026, 6, 18),
            City = "Osijek",
            Teams = teams.Where(t => t.Id is 7 or 8 or 9).ToList(),
            Administrators = users.Where(u => u.Id == 1).ToList()
        }
    };

    foreach (var team in teams)
    {
        team.Competitions = competitions.Where(c => c.Teams.Any(t => t.Id == team.Id)).ToList();
    }

    var groups = new List<GroupPhaseGroup>
    {
        new() { Id = 1, Name = "Group A", CompetitionId = 1, Teams = teams.Where(t => t.Id is 1 or 2).ToList() },
        new() { Id = 2, Name = "Group B", CompetitionId = 1, Teams = teams.Where(t => t.Id == 3).ToList() },
        new() { Id = 3, Name = "Group A", CompetitionId = 2, Teams = teams.Where(t => t.Id is 4 or 5).ToList() },
        new() { Id = 4, Name = "Group B", CompetitionId = 2, Teams = teams.Where(t => t.Id == 6).ToList() },
        new() { Id = 5, Name = "Group A", CompetitionId = 3, Teams = teams.Where(t => t.Id is 7 or 8).ToList() },
        new() { Id = 6, Name = "Group B", CompetitionId = 3, Teams = teams.Where(t => t.Id == 9).ToList() }
    };

    var matches = new List<Match>
    {
        new() { Id = 1, CompetitionId = 1, GroupId = 1, RoundNumber = 1, Kickoff = new DateTime(2026, 4, 10, 17, 0, 0), HomeTeamId = 1, AwayTeamId = 2, HomeScore = 29, AwayScore = 27, MaintenanceHall = "Arena Trešnjevka", Status = MatchStatus.Finished },
        new() { Id = 2, CompetitionId = 1, GroupId = 1, RoundNumber = 2, Kickoff = new DateTime(2026, 4, 12, 18, 0, 0), HomeTeamId = 2, AwayTeamId = 1, HomeScore = 24, AwayScore = 24, MaintenanceHall = "Dom Sportova", Status = MatchStatus.Finished },
        new() { Id = 3, CompetitionId = 1, GroupId = 2, RoundNumber = 1, Kickoff = new DateTime(2026, 4, 13, 19, 0, 0), HomeTeamId = 3, AwayTeamId = 1, HomeScore = 20, AwayScore = 31, MaintenanceHall = "SD Sisak", Status = MatchStatus.Finished },
        new() { Id = 4, CompetitionId = 2, GroupId = 3, RoundNumber = 1, Kickoff = new DateTime(2026, 5, 3, 17, 30, 0), HomeTeamId = 4, AwayTeamId = 5, HomeScore = 22, AwayScore = 26, MaintenanceHall = "Borovo Hall", Status = MatchStatus.Finished },
        new() { Id = 5, CompetitionId = 2, GroupId = 3, RoundNumber = 2, Kickoff = new DateTime(2026, 5, 6, 18, 15, 0), HomeTeamId = 5, AwayTeamId = 4, HomeScore = 28, AwayScore = 25, MaintenanceHall = "Gripe", Status = MatchStatus.Finished },
        new() { Id = 6, CompetitionId = 2, GroupId = 4, RoundNumber = 1, Kickoff = new DateTime(2026, 5, 7, 20, 0, 0), HomeTeamId = 6, AwayTeamId = 4, HomeScore = 18, AwayScore = 18, MaintenanceHall = "Jazine", Status = MatchStatus.Finished },
        new() { Id = 7, CompetitionId = 3, GroupId = 5, RoundNumber = 1, Kickoff = new DateTime(2026, 6, 1, 17, 0, 0), HomeTeamId = 7, AwayTeamId = 8, HomeScore = 30, AwayScore = 27, MaintenanceHall = "Gradski vrt", Status = MatchStatus.Finished },
        new() { Id = 8, CompetitionId = 3, GroupId = 5, RoundNumber = 2, Kickoff = new DateTime(2026, 6, 5, 18, 0, 0), HomeTeamId = 8, AwayTeamId = 7, HomeScore = 23, AwayScore = 22, MaintenanceHall = "Mladost", Status = MatchStatus.Finished },
        new() { Id = 9, CompetitionId = 3, GroupId = 6, RoundNumber = 1, Kickoff = new DateTime(2026, 6, 6, 19, 0, 0), HomeTeamId = 9, AwayTeamId = 7, HomeScore = 21, AwayScore = 29, MaintenanceHall = "Veli Joze", Status = MatchStatus.Finished }
    };

    foreach (var group in groups)
    {
        group.Matches = matches.Where(m => m.GroupId == group.Id).ToList();
    }

    foreach (var competition in competitions)
    {
        competition.Groups = groups.Where(g => g.CompetitionId == competition.Id).ToList();
    }

    return new LabData(competitions, teams, players, groups, matches, users);
}

static void RunLabQueries(LabData data)
{
    var competitionsInSpring = data.Competitions
        .Where(c => c.StartDate.Month is >= 4 and <= 6)
        .OrderBy(c => c.StartDate)
        .ToList();

    var teamsByAverageGoals = data.Teams
        .Select(team => new
        {
            Team = team.Name,
            AvgGoals = data.Matches
                .Where(m => m.HomeTeamId == team.Id || m.AwayTeamId == team.Id)
                .Select(m => m.HomeTeamId == team.Id ? m.HomeScore : m.AwayScore)
                .DefaultIfEmpty(0)
                .Average()
        })
        .OrderByDescending(x => x.AvgGoals)
        .ToList();

    var topScorers = data.Players
        .OrderByDescending(p => p.GoalsScored)
        .ThenByDescending(p => p.Assists)
        .Take(5)
        .Select(p => new { p.FullName, p.GoalsScored, p.Assists })
        .ToList();

    var competitionMatchCounts = data.Competitions
        .Select(c => new
        {
            Competition = c.Name,
            Matches = data.Matches.Count(m => m.CompetitionId == c.Id)
        })
        .OrderByDescending(x => x.Matches)
        .ToList();

    var managerCoverage = data.Users
        .Where(u => u.Role is UserRole.Admin or UserRole.TournamentManager)
        .Select(u => new
        {
            Manager = u.DisplayName,
            ManagedCompetitions = data.Competitions
                .Where(c => u.ManagedCompetitionIds.Contains(c.Id) || c.Administrators.Any(a => a.Id == u.Id))
                .Select(c => c.Name)
                .ToList()
        })
        .ToList();

    var singleCompetitionByCity = data.Competitions
        .SingleOrDefault(c => c.City == "Split");

    Console.WriteLine($"Lab datasets seeded: {data.Competitions.Count} competitions, {data.Teams.Count} teams, {data.Players.Count} players, {data.Matches.Count} matches.");
    Console.WriteLine($"Spring competitions: {competitionsInSpring.Count}");
    Console.WriteLine($"Top scorer: {topScorers.FirstOrDefault()?.FullName ?? "N/A"}");
    Console.WriteLine($"Competition in Split: {singleCompetitionByCity?.Name ?? "None"}");
    Console.WriteLine($"Manager coverage records: {managerCoverage.Count}");
    Console.WriteLine($"Teams ranked by avg goals records: {teamsByAverageGoals.Count}");
    Console.WriteLine($"Competition match count records: {competitionMatchCounts.Count}");
}

static async Task DemonstrateAsyncAwait()
{
    Console.WriteLine("Async demo started...");
    await Task.Delay(250);
    Console.WriteLine("Async demo completed.");
}

public record LabData(
    List<Competition> Competitions,
    List<Team> Teams,
    List<Player> Players,
    List<GroupPhaseGroup> Groups,
    List<Match> Matches,
    List<AppUser> Users);
