using HandballCompetitionManager.Models;
using HandballCompetitionManager.Repositories;
using HandballCompetitionManager.Repositories.Mock;
using HandballCompetitionManager.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<HandballDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("HandballDbContext")));

builder.Services
    .AddIdentity<AppUser, IdentityRole<int>>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<HandballDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<PlayerRepository>();
builder.Services.AddScoped<MatchRepository>();
builder.Services.AddScoped<TeamRepository>();
builder.Services.AddScoped<GroupPhaseRepository>();
builder.Services.AddScoped<CompetitionRepository>();
builder.Services.AddScoped<AppUserRepository>();

builder.Services.AddSingleton<PlayerMockRepository>();
builder.Services.AddSingleton<MatchMockRepository>();
builder.Services.AddSingleton<TeamMockRepository>();
builder.Services.AddSingleton<GroupPhaseMockRepository>();
builder.Services.AddSingleton<CompetitionMockRepository>();
builder.Services.AddSingleton<AppUserMockRepository>();

var app = builder.Build();

await SeedIdentityRolesAsync(app.Services);
await SeedInitialDataAsync(app.Services);

var labData = BuildLabData();
RunLabQueries(labData);
await DemonstrateAsyncAwait();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();

app.Run();

static async Task SeedIdentityRolesAsync(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    foreach (var role in new[] { "Admin", "Manager", "Coach" })
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole<int>(role));
        }
    }

    var adminEmail = "admin@handball.local";
    var admin = await userManager.FindByEmailAsync(adminEmail);

    if (admin == null)
    {
        admin = new AppUser
        {
            UserName = "admin",
            DisplayName = "Admin User",
            Email = adminEmail,
            EmailConfirmed = true,
            Role = UserRole.Admin,
            OIB = "12345678901",
            JMBG = "1234567890123",
            DateOfBirth = new DateTime(1985, 2, 12),
            CreatedAt = DateTime.UtcNow
        };

        var createResult = await userManager.CreateAsync(admin, "Admin123!");
        if (!createResult.Succeeded)
        {
            Console.WriteLine($"Admin seed failed: {string.Join(", ", createResult.Errors.Select(error => error.Description))}");
            return;
        }
    }
    else if (string.IsNullOrWhiteSpace(admin.PasswordHash))
    {
        var passwordResult = await userManager.AddPasswordAsync(admin, "Admin123!");
        if (!passwordResult.Succeeded)
        {
            Console.WriteLine($"Admin password seed failed: {string.Join(", ", passwordResult.Errors.Select(error => error.Description))}");
        }
    }

    if (!await userManager.IsInRoleAsync(admin, "Admin"))
    {
        await userManager.AddToRoleAsync(admin, "Admin");
    }
}

static async Task SeedInitialDataAsync(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<HandballDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    var admin = await EnsureUserAsync(
        userManager,
        username: "dean",
        email: "vidovicdean@gmail.com",
        displayName: "Dean Vidovic",
        role: UserRole.Admin,
        identityRole: "Admin",
        password: "Dean123!",
        oib: "11111111111",
        jmbg: "1111111111111");

    var manager = await EnsureUserAsync(
        userManager,
        username: "manager.zagreb",
        email: "manager@handball.local",
        displayName: "Zagreb Manager",
        role: UserRole.TournamentManager,
        identityRole: "Manager",
        password: "Manager123!",
        oib: "22222222222",
        jmbg: "2222222222222");

    var coachZagreb = await EnsureUserAsync(
        userManager,
        username: "coach.horvat",
        email: "coach.horvat@handball.local",
        displayName: "Coach Horvat",
        role: UserRole.Coach,
        identityRole: "Coach",
        password: "Coach123!",
        oib: "33333333333",
        jmbg: "3333333333333");

    var coachSplit = await EnsureUserAsync(
        userManager,
        username: "coach.vukovic",
        email: "coach.vukovic@handball.local",
        displayName: "Coach Vukovic",
        role: UserRole.Coach,
        identityRole: "Coach",
        password: "Coach123!",
        oib: "44444444444",
        jmbg: "4444444444444");

    await EnsureUserAsync(
        userManager,
        username: "guest.user",
        email: "guest@handball.local",
        displayName: "Guest User",
        role: UserRole.Guest,
        identityRole: null,
        password: "Guest123!",
        oib: "55555555555",
        jmbg: "5555555555555");

    if (context.Teams.Any() || context.Competitions.Any())
    {
        return;
    }

    var teams = new List<Team>
    {
        new() { Name = "RK Zagreb", CoachName = coachZagreb.DisplayName, HomeCity = "Zagreb", FoundedYear = 1922, HomeArena = "Arena Zagreb" },
        new() { Name = "RK Metalac", CoachName = coachZagreb.DisplayName, HomeCity = "Zagreb", FoundedYear = 1965, HomeArena = "Kutija Sibica" },
        new() { Name = "RK Split", CoachName = coachSplit.DisplayName, HomeCity = "Split", FoundedYear = 1958, HomeArena = "SC Gripe" },
        new() { Name = "RK Osijek", CoachName = "Coach Kovac", HomeCity = "Osijek", FoundedYear = 1970, HomeArena = "Gradski vrt" }
    };

    var competitions = new List<Competition>
    {
        new()
        {
            Name = "Zagreb Handball Cup",
            Season = "2025/2026",
            City = "Zagreb",
            StartDate = new DateTime(2026, 7, 1),
            EndDate = new DateTime(2026, 7, 10),
            Teams = teams.Where(team => team.Name is "RK Zagreb" or "RK Metalac" or "RK Split").ToList(),
            Administrators = new List<AppUser> { admin, manager }
        },
        new()
        {
            Name = "Adriatic League",
            Season = "2025/2026",
            City = "Split",
            StartDate = new DateTime(2026, 8, 5),
            EndDate = new DateTime(2026, 8, 18),
            Teams = teams.Where(team => team.Name is "RK Split" or "RK Osijek").ToList(),
            Administrators = new List<AppUser> { admin }
        }
    };

    var players = new List<Player>
    {
        new() { FirstName = "Marko", LastName = "Horvat", BirthDate = new DateTime(1998, 5, 12), JerseyNumber = 1, Position = PlayerPosition.Goalkeeper, Team = teams[0], GoalsScored = 0, Assists = 2 },
        new() { FirstName = "Ivan", LastName = "Novak", BirthDate = new DateTime(2000, 3, 20), JerseyNumber = 9, Position = PlayerPosition.LeftBack, Team = teams[0], GoalsScored = 18, Assists = 7 },
        new() { FirstName = "Luka", LastName = "Basic", BirthDate = new DateTime(1999, 8, 2), JerseyNumber = 7, Position = PlayerPosition.RightWing, Team = teams[1], GoalsScored = 14, Assists = 5 },
        new() { FirstName = "Ante", LastName = "Maric", BirthDate = new DateTime(2001, 11, 9), JerseyNumber = 10, Position = PlayerPosition.CenterBack, Team = teams[2], GoalsScored = 22, Assists = 11 },
        new() { FirstName = "Petar", LastName = "Kovac", BirthDate = new DateTime(1997, 1, 15), JerseyNumber = 5, Position = PlayerPosition.Pivot, Team = teams[3], GoalsScored = 12, Assists = 3 }
    };

    var groups = new List<GroupPhase>
    {
        new() { Name = "Group A", Competition = competitions[0], Teams = teams.Where(team => team.Name is "RK Zagreb" or "RK Metalac").ToList() },
        new() { Name = "Group B", Competition = competitions[0], Teams = teams.Where(team => team.Name == "RK Split").ToList() },
        new() { Name = "Group A", Competition = competitions[1], Teams = teams.Where(team => team.Name is "RK Split" or "RK Osijek").ToList() }
    };

    var matches = new List<Match>
    {
        new() { Competition = competitions[0], GroupPhase = groups[0], RoundNumber = 1, HomeTeam = teams[0], AwayTeam = teams[1], HomeScore = 29, AwayScore = 24, Kickoff = new DateTime(2026, 7, 1, 18, 0, 0), MaintenanceHall = "Arena Zagreb", Status = MatchStatus.Finished },
        new() { Competition = competitions[0], GroupPhase = groups[1], RoundNumber = 1, HomeTeam = teams[2], AwayTeam = teams[0], HomeScore = 0, AwayScore = 0, Kickoff = new DateTime(2026, 7, 3, 19, 0, 0), MaintenanceHall = "SC Gripe", Status = MatchStatus.Scheduled },
        new() { Competition = competitions[1], GroupPhase = groups[2], RoundNumber = 1, HomeTeam = teams[2], AwayTeam = teams[3], HomeScore = 31, AwayScore = 28, Kickoff = new DateTime(2026, 8, 5, 20, 0, 0), MaintenanceHall = "SC Gripe", Status = MatchStatus.Finished }
    };

    context.Teams.AddRange(teams);
    context.Competitions.AddRange(competitions);
    context.Players.AddRange(players);
    context.GroupPhases.AddRange(groups);
    context.Matches.AddRange(matches);
    await context.SaveChangesAsync();
}

static async Task<AppUser> EnsureUserAsync(
    UserManager<AppUser> userManager,
    string username,
    string email,
    string displayName,
    UserRole role,
    string? identityRole,
    string password,
    string oib,
    string jmbg)
{
    var user = await userManager.FindByEmailAsync(email)
        ?? await userManager.FindByNameAsync(username);

    if (user == null)
    {
        user = new AppUser
        {
            UserName = username,
            DisplayName = displayName,
            Email = email,
            EmailConfirmed = true,
            Role = role,
            OIB = oib,
            JMBG = jmbg,
            CreatedAt = DateTime.UtcNow
        };

        var createResult = await userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
        {
            throw new InvalidOperationException($"Seed user {email} failed: {string.Join(", ", createResult.Errors.Select(error => error.Description))}");
        }
    }
    else
    {
        user.UserName = username;
        user.DisplayName = displayName;
        user.Email = email;
        user.EmailConfirmed = true;
        user.Role = role;
        user.OIB = oib;
        user.JMBG = jmbg;
        user.DeletedAt = null;
        await userManager.UpdateAsync(user);

        if (!await userManager.HasPasswordAsync(user))
        {
            await userManager.AddPasswordAsync(user, password);
        }
    }

    if (!string.IsNullOrWhiteSpace(identityRole) && !await userManager.IsInRoleAsync(user, identityRole))
    {
        await userManager.AddToRoleAsync(user, identityRole);
    }

    return user;
}

static LabData BuildLabData()
{
    var users = new List<AppUser>
    {
        new()
        {
            Id = 1,
            UserName = "admin",
            DisplayName = "System Admin",
            Email = "admin@hcm.local",
            Role = UserRole.Admin,
            CreatedAt = new DateTime(2026, 1, 10)
        },
        new()
        {
            Id = 2,
            UserName = "tm.zagreb",
            DisplayName = "Zagreb Manager",
            Email = "tm.zagreb@hcm.local",
            Role = UserRole.TournamentManager,
            CreatedAt = new DateTime(2026, 1, 15),
            ManagedCompetitions = new List<Competition>()
        },
        new()
        {
            Id = 3,
            UserName = "coach.metalac",
            DisplayName = "Coach Metalac",
            Email = "coach.metalac@hcm.local",
            Role = UserRole.Coach,
            CreatedAt = new DateTime(2026, 2, 1),
            ManagedCompetitions = new List<Competition>()
        },
        new()
        {
            Id = 4,
            UserName = "guest.one",
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
            CoachName = "Ivan Horvat",
            HomeCity = "Zagreb",
            FoundedYear = 1965,
            HomeArena = "Arena Trešnjevka"
        },
        new()
        {
            Id = 2,
            Name = "RK Medvescak",
            CoachName = "Luka Gajic",
            HomeCity = "Zagreb",
            FoundedYear = 1973,
            HomeArena = "Dom Sportova"
        },
        new()
        {
            Id = 3,
            Name = "RK Sisak",
            CoachName = "Petar Radic",
            HomeCity = "Sisak",
            FoundedYear = 1980,
            HomeArena = "SD Sisak"
        },
        new()
        {
            Id = 4,
            Name = "RK Vukovar",
            CoachName = "Marko Kolar",
            HomeCity = "Vukovar",
            FoundedYear = 1979,
            HomeArena = "Borovo Hall"
        },
        new()
        {
            Id = 5,
            Name = "RK Split",
            CoachName = "Josip Marin",
            HomeCity = "Split",
            FoundedYear = 1958,
            HomeArena = "Gripe"
        },
        new()
        {
            Id = 6,
            Name = "RK Zadar",
            CoachName = "Ante Klaric",
            HomeCity = "Zadar",
            FoundedYear = 1969,
            HomeArena = "Jazine"
        },
        new()
        {
            Id = 7,
            Name = "RK Osijek",
            CoachName = "Dario Bulat",
            HomeCity = "Osijek",
            FoundedYear = 1970,
            HomeArena = "Gradski vrt"
        },
        new()
        {
            Id = 8,
            Name = "RK Karlovac",
            CoachName = "Tomislav Knez",
            HomeCity = "Karlovac",
            FoundedYear = 1975,
            HomeArena = "Mladost"
        },
        new()
        {
            Id = 9,
            Name = "RK Porec",
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

    var groups = new List<GroupPhase>
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
                .Where(c => c.Administrators.Any(a => a.Id == u.Id))
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
    List<GroupPhase> Groups,
    List<Match> Matches,
    List<AppUser> Users);

public partial class Program
{
}
