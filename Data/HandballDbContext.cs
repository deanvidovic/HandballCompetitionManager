using Microsoft.EntityFrameworkCore;
using HandballCompetitionManager.Models;

namespace HandballCompetitionManager.Data;

public class HandballDbContext : DbContext
{
    public HandballDbContext(DbContextOptions<HandballDbContext> options) : base(options)
    {
    }

    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Competition> Competitions { get; set; }
    public DbSet<GroupPhase> GroupPhases { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Team> Teams { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure many-to-many: Team <-> Competition
        modelBuilder.Entity<Team>()
            .HasMany(t => t.Competitions)
            .WithMany(c => c.Teams)
            .UsingEntity(j => j.ToTable("TeamCompetitions"));

        // Configure many-to-many: Competition <-> AppUser (Administrators)
        modelBuilder.Entity<Competition>()
            .HasMany(c => c.Administrators)
            .WithMany(u => u.ManagedCompetitions)
            .UsingEntity(j => j.ToTable("CompetitionAdministrators"));

        // Configure one-to-many: Team -> Player
        modelBuilder.Entity<Player>()
            .HasOne(p => p.Team)
            .WithMany(t => t.Players)
            .HasForeignKey(p => p.TeamId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure one-to-many: Competition -> GroupPhase
        modelBuilder.Entity<GroupPhase>()
            .HasOne(g => g.Competition)
            .WithMany(c => c.Groups)
            .HasForeignKey(g => g.CompetitionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure many-to-many: GroupPhase <-> Team
        modelBuilder.Entity<GroupPhase>()
            .HasMany(g => g.Teams)
            .WithMany(t => t.GroupPhases)
            .UsingEntity(j => j.ToTable("GroupPhaseTeams"));

        // Configure one-to-many: GroupPhase -> Match
        modelBuilder.Entity<Match>()
            .HasOne(m => m.GroupPhase)
            .WithMany(g => g.Matches)
            .HasForeignKey(m => m.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Match -> Competition relationship
        modelBuilder.Entity<Match>()
            .HasOne(m => m.Competition)
            .WithMany()
            .HasForeignKey(m => m.CompetitionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Match -> HomeTeam relationship
        modelBuilder.Entity<Match>()
            .HasOne(m => m.HomeTeam)
            .WithMany()
            .HasForeignKey(m => m.HomeTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Match -> AwayTeam relationship
        modelBuilder.Entity<Match>()
            .HasOne(m => m.AwayTeam)
            .WithMany()
            .HasForeignKey(m => m.AwayTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed Teams
        modelBuilder.Entity<Team>().HasData(
            new Team { Id = 1, Name = "Zagreb Handball", CoachName = "Coach Horvat", HomeCity = "Zagreb", FoundedYear = 2005, HomeArena = "Arena Zagreb" },
            new Team { Id = 2, Name = "Split Warriors", CoachName = "Coach Vukovic", HomeCity = "Split", FoundedYear = 2008, HomeArena = "Gradski Vrt" },
            new Team { Id = 3, Name = "Rijeka Sharks", CoachName = "Coach Ivanovic", HomeCity = "Rijeka", FoundedYear = 2003, HomeArena = "Mladost Arena" },
            new Team { Id = 4, Name = "Osijek Titans", CoachName = "Coach Grgic", HomeCity = "Osijek", FoundedYear = 2010, HomeArena = "Gradski Vrt Osijek" },
            new Team { Id = 5, Name = "Zadar Eagles", CoachName = "Coach Milic", HomeCity = "Zadar", FoundedYear = 2006, HomeArena = "Zadar Arena" }
        );

        // Seed Players
        modelBuilder.Entity<Player>().HasData(
            // Zagreb Handball
            new Player { Id = 1, FirstName = "Marko", LastName = "Horvat", BirthDate = new DateTime(1990, 5, 15), Position = PlayerPosition.Goalkeeper, TeamId = 1, JerseyNumber = 1, GoalsScored = 0, Assists = 0 },
            new Player { Id = 2, FirstName = "Ivan", LastName = "Novak", BirthDate = new DateTime(1995, 3, 20), Position = PlayerPosition.LeftWing, TeamId = 1, JerseyNumber = 7, GoalsScored = 45, Assists = 12 },
            new Player { Id = 3, FirstName = "Petar", LastName = "Milic", BirthDate = new DateTime(1993, 7, 10), Position = PlayerPosition.RightWing, TeamId = 1, JerseyNumber = 8, GoalsScored = 52, Assists = 8 },
            new Player { Id = 4, FirstName = "Damir", LastName = "Kitic", BirthDate = new DateTime(1992, 1, 25), Position = PlayerPosition.Pivot, TeamId = 1, JerseyNumber = 10, GoalsScored = 38, Assists = 15 },
            // Split Warriors
            new Player { Id = 5, FirstName = "Ante", LastName = "Vukovic", BirthDate = new DateTime(1988, 11, 3), Position = PlayerPosition.Goalkeeper, TeamId = 2, JerseyNumber = 1, GoalsScored = 0, Assists = 0 },
            new Player { Id = 6, FirstName = "Luka", LastName = "Bevandic", BirthDate = new DateTime(1996, 8, 18), Position = PlayerPosition.RightWing, TeamId = 2, JerseyNumber = 9, GoalsScored = 48, Assists = 10 },
            new Player { Id = 7, FirstName = "Stipe", LastName = "Mandic", BirthDate = new DateTime(1994, 4, 22), Position = PlayerPosition.Pivot, TeamId = 2, JerseyNumber = 11, GoalsScored = 35, Assists = 20 },
            // Rijeka Sharks
            new Player { Id = 8, FirstName = "Goran", LastName = "Ivanovic", BirthDate = new DateTime(1989, 9, 5), Position = PlayerPosition.Goalkeeper, TeamId = 3, JerseyNumber = 1, GoalsScored = 0, Assists = 0 },
            new Player { Id = 9, FirstName = "Nikola", LastName = "Salic", BirthDate = new DateTime(1997, 2, 14), Position = PlayerPosition.LeftWing, TeamId = 3, JerseyNumber = 5, GoalsScored = 42, Assists = 11 },
            // Osijek Titans
            new Player { Id = 10, FirstName = "Dino", LastName = "Grgic", BirthDate = new DateTime(1991, 6, 8), Position = PlayerPosition.Pivot, TeamId = 4, JerseyNumber = 12, GoalsScored = 40, Assists = 18 },
            // Zadar Eagles
            new Player { Id = 11, FirstName = "Zoran", LastName = "Milic", BirthDate = new DateTime(1994, 12, 30), Position = PlayerPosition.RightWing, TeamId = 5, JerseyNumber = 13, GoalsScored = 46, Assists = 9 }
        );

        // Seed AppUsers
        modelBuilder.Entity<AppUser>().HasData(
            new AppUser { Id = 1, Username = "admin", DisplayName = "Admin User", Email = "admin@handball.local", Role = UserRole.Admin, DateOfBirth = new DateTime(1985, 2, 12), CreatedAt = new DateTime(2025, 5, 4) },
            new AppUser { Id = 2, Username = "coach_horvat", DisplayName = "Coach Horvat", Email = "coach1@handball.local", Role = UserRole.Coach, DateOfBirth = new DateTime(1979, 7, 21), CreatedAt = new DateTime(2025, 5, 4) },
            new AppUser { Id = 3, Username = "coach_vukovic", DisplayName = "Coach Vukovic", Email = "coach2@handball.local", Role = UserRole.Coach, DateOfBirth = new DateTime(1982, 10, 3), CreatedAt = new DateTime(2025, 5, 4) },
            new AppUser { Id = 4, Username = "guest_user", DisplayName = "Guest User", Email = "guest@handball.local", Role = UserRole.Guest, DateOfBirth = new DateTime(1993, 1, 16), CreatedAt = new DateTime(2025, 5, 4) }
        );

        // Seed Competitions
        modelBuilder.Entity<Competition>().HasData(
            new Competition { Id = 1, Name = "Croatian League 2025", Season = "2024/2025", StartDate = new DateTime(2025, 5, 9), EndDate = new DateTime(2025, 11, 27), City = "Zagreb" },
            new Competition { Id = 2, Name = "Croatian Cup 2025", Season = "2024/2025", StartDate = new DateTime(2025, 5, 14), EndDate = new DateTime(2025, 8, 2), City = "Split" },
            new Competition { Id = 3, Name = "Regional Championship", Season = "2025", StartDate = new DateTime(2025, 5, 24), EndDate = new DateTime(2025, 10, 2), City = "Zagreb" }
        );

        // Seed GroupPhases
        modelBuilder.Entity<GroupPhase>().HasData(
            new GroupPhase { Id = 1, Name = "Group A", CompetitionId = 1 },
            new GroupPhase { Id = 2, Name = "Group B", CompetitionId = 1 },
            new GroupPhase { Id = 3, Name = "Finals", CompetitionId = 2 }
        );

        // Seed Matches
        modelBuilder.Entity<Match>().HasData(
            new Match { Id = 1, CompetitionId = 1, GroupId = 1, HomeTeamId = 1, AwayTeamId = 2, HomeScore = 28, AwayScore = 25, Kickoff = new DateTime(2025, 5, 5, 19, 0, 0), Status = MatchStatus.Scheduled },
            new Match { Id = 2, CompetitionId = 1, GroupId = 1, HomeTeamId = 3, AwayTeamId = 4, HomeScore = 0, AwayScore = 0, Kickoff = new DateTime(2025, 5, 6, 19, 0, 0), Status = MatchStatus.Scheduled },
            new Match { Id = 3, CompetitionId = 1, GroupId = 2, HomeTeamId = 5, AwayTeamId = 1, HomeScore = 0, AwayScore = 0, Kickoff = new DateTime(2025, 5, 7, 19, 0, 0), Status = MatchStatus.Scheduled },
            new Match { Id = 4, CompetitionId = 2, GroupId = 3, HomeTeamId = 2, AwayTeamId = 3, HomeScore = 31, AwayScore = 29, Kickoff = new DateTime(2025, 5, 3, 18, 0, 0), Status = MatchStatus.Finished }
        );
    }
}
