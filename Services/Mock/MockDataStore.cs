using HandballCompetitionManager.ViewModels;

namespace HandballCompetitionManager.Services.Mock;

internal static class MockDataStore
{
    internal static readonly IReadOnlyList<TournamentCardViewModel> Tournaments =
    [
        new()
        {
            Id = 1,
            Name = "Croatia Handball Cup 2026",
            Status = "Active",
            Location = "Zagreb, Croatia",
            StartDate = "June 18, 2026",
            EndDate = "June 22, 2026",
            TeamCount = 8,
            MatchCount = 15,
            CurrentPhase = "Group Phase",
            Description = "National senior tournament with group standings, live results, and player statistics."
        },
        new()
        {
            Id = 2,
            Name = "Adriatic Youth Challenge",
            Status = "Upcoming",
            Location = "Split, Croatia",
            StartDate = "July 8, 2026",
            EndDate = "July 12, 2026",
            TeamCount = 4,
            MatchCount = 6,
            CurrentPhase = "Registration",
            Description = "Youth competition for coastal clubs with public schedules and team profiles."
        },
        new()
        {
            Id = 3,
            Name = "Central Europe Invitational",
            Status = "Completed",
            Location = "Varazdin, Croatia",
            StartDate = "May 14, 2026",
            EndDate = "May 16, 2026",
            TeamCount = 4,
            MatchCount = 6,
            CurrentPhase = "Completed",
            Description = "Completed invitational bracket with results, standings, and final statistics."
        },
        new()
        {
            Id = 4,
            Name = "Slavonia Open Tournament",
            Status = "Active",
            Location = "Osijek, Croatia",
            StartDate = "June 22, 2026",
            EndDate = "June 25, 2026",
            TeamCount = 4,
            MatchCount = 6,
            CurrentPhase = "Group Phase",
            Description = "Regional club tournament with active match reporting and public standings."
        }
    ];

    internal static readonly IReadOnlyList<TeamSummaryViewModel> Teams =
    [
        new() { Id = 1, TournamentId = 1, Name = "RK Zagreb", City = "Zagreb", Country = "Croatia", CoachName = "Ivan Horvat" },
        new() { Id = 2, TournamentId = 1, Name = "RK Nexe", City = "Nasice", Country = "Croatia", CoachName = "Marko Kovac" },
        new() { Id = 3, TournamentId = 1, Name = "RK Split", City = "Split", Country = "Croatia", CoachName = "Ante Maric" },
        new() { Id = 4, TournamentId = 1, Name = "RK Varazdin", City = "Varazdin", Country = "Croatia", CoachName = "Petar Novak" },
        new() { Id = 21, TournamentId = 1, Name = "RK Porec", City = "Porec", Country = "Croatia", CoachName = "Mladen Tomic" },
        new() { Id = 22, TournamentId = 1, Name = "RK Sesvete", City = "Sesvete", Country = "Croatia", CoachName = "Davor Kralj" },
        new() { Id = 23, TournamentId = 1, Name = "RK Metkovic", City = "Metkovic", Country = "Croatia", CoachName = "Nikola Juric" },
        new() { Id = 24, TournamentId = 1, Name = "RK Bjelovar", City = "Bjelovar", Country = "Croatia", CoachName = "Hrvoje Matos" },
        new() { Id = 5, TournamentId = 2, Name = "RK Zadar", City = "Zadar", Country = "Croatia", CoachName = "Dario Bilic" },
        new() { Id = 6, TournamentId = 2, Name = "RK Dubrovnik", City = "Dubrovnik", Country = "Croatia", CoachName = "Luka Peric" },
        new() { Id = 7, TournamentId = 3, Name = "RK Celje", City = "Celje", Country = "Slovenia", CoachName = "Matej Kralj" },
        new() { Id = 8, TournamentId = 4, Name = "RK Osijek", City = "Osijek", Country = "Croatia", CoachName = "Stjepan Juric" }
    ];

    internal static readonly IReadOnlyList<PlayerSummaryViewModel> Players =
    [
        new() { Id = 1, TeamId = 1, FullName = "Luka Vidovic", ShirtNumber = 7, Position = "Left Back", IsCaptain = true },
        new() { Id = 2, TeamId = 1, FullName = "Mateo Kolar", ShirtNumber = 11, Position = "Right Wing", IsCaptain = false },
        new() { Id = 3, TeamId = 1, FullName = "Filip Barisic", ShirtNumber = 21, Position = "Goalkeeper", IsCaptain = false },
        new() { Id = 4, TeamId = 2, FullName = "Ivan Radic", ShirtNumber = 9, Position = "Center Back", IsCaptain = true },
        new() { Id = 5, TeamId = 2, FullName = "Tomislav Bencic", ShirtNumber = 14, Position = "Pivot", IsCaptain = false },
        new() { Id = 6, TeamId = 2, FullName = "Nikola Pavic", ShirtNumber = 19, Position = "Left Wing", IsCaptain = false },
        new() { Id = 7, TeamId = 3, FullName = "Duje Matic", ShirtNumber = 5, Position = "Left Wing", IsCaptain = true },
        new() { Id = 8, TeamId = 3, FullName = "Marin Grgic", ShirtNumber = 18, Position = "Right Back", IsCaptain = false },
        new() { Id = 9, TeamId = 3, FullName = "Roko Simic", ShirtNumber = 16, Position = "Goalkeeper", IsCaptain = false },
        new() { Id = 10, TeamId = 4, FullName = "Leon Novak", ShirtNumber = 10, Position = "Center Back", IsCaptain = true },
        new() { Id = 11, TeamId = 4, FullName = "Karlo Horvat", ShirtNumber = 22, Position = "Pivot", IsCaptain = false },
        new() { Id = 12, TeamId = 4, FullName = "Borna Vuk", ShirtNumber = 4, Position = "Defender", IsCaptain = false },
        new() { Id = 13, TeamId = 5, FullName = "Toni Skara", ShirtNumber = 6, Position = "Left Back", IsCaptain = true },
        new() { Id = 14, TeamId = 5, FullName = "Josip Klaric", ShirtNumber = 13, Position = "Right Wing", IsCaptain = false },
        new() { Id = 15, TeamId = 6, FullName = "Miro Batinic", ShirtNumber = 8, Position = "Center Back", IsCaptain = true },
        new() { Id = 16, TeamId = 6, FullName = "Noel Cavar", ShirtNumber = 24, Position = "Goalkeeper", IsCaptain = false },
        new() { Id = 17, TeamId = 7, FullName = "Jan Novak", ShirtNumber = 15, Position = "Pivot", IsCaptain = true },
        new() { Id = 18, TeamId = 7, FullName = "Miha Zupan", ShirtNumber = 20, Position = "Right Back", IsCaptain = false },
        new() { Id = 19, TeamId = 8, FullName = "Domagoj Sakic", ShirtNumber = 3, Position = "Left Wing", IsCaptain = true },
        new() { Id = 20, TeamId = 8, FullName = "Fran Milic", ShirtNumber = 17, Position = "Left Back", IsCaptain = false },
        new() { Id = 21, TeamId = 21, FullName = "Lovro Krizan", ShirtNumber = 6, Position = "Center Back", IsCaptain = true },
        new() { Id = 22, TeamId = 21, FullName = "Noa Matosevic", ShirtNumber = 12, Position = "Goalkeeper", IsCaptain = false },
        new() { Id = 23, TeamId = 22, FullName = "Patrik Vidak", ShirtNumber = 15, Position = "Right Back", IsCaptain = true },
        new() { Id = 24, TeamId = 22, FullName = "Emanuel Grbic", ShirtNumber = 2, Position = "Right Wing", IsCaptain = false },
        new() { Id = 25, TeamId = 23, FullName = "Marin Vukovic", ShirtNumber = 9, Position = "Left Back", IsCaptain = true },
        new() { Id = 26, TeamId = 23, FullName = "Kresimir Bago", ShirtNumber = 18, Position = "Pivot", IsCaptain = false },
        new() { Id = 27, TeamId = 24, FullName = "Tin Markovic", ShirtNumber = 11, Position = "Center Back", IsCaptain = true },
        new() { Id = 28, TeamId = 24, FullName = "Bruno Cacic", ShirtNumber = 23, Position = "Left Wing", IsCaptain = false }
    ];

    internal static readonly IReadOnlyList<MatchSummaryViewModel> Matches =
    [
        new() { Id = 1, TournamentId = 1, HomeTeamId = 1, AwayTeamId = 3, HomeTeamName = "RK Zagreb", AwayTeamName = "RK Split", RoundName = "Group A", VenueName = "Arena Zagreb", ScheduledAt = new DateTime(2026, 6, 18, 18, 0, 0), Status = "Completed", HomeScore = 31, AwayScore = 29 },
        new() { Id = 2, TournamentId = 1, HomeTeamId = 2, AwayTeamId = 4, HomeTeamName = "RK Nexe", AwayTeamName = "RK Varazdin", RoundName = "Group A", VenueName = "Arena Zagreb", ScheduledAt = new DateTime(2026, 6, 18, 20, 0, 0), Status = "Completed", HomeScore = 25, AwayScore = 25 },
        new() { Id = 3, TournamentId = 1, HomeTeamId = 1, AwayTeamId = 4, HomeTeamName = "RK Zagreb", AwayTeamName = "RK Varazdin", RoundName = "Group A", VenueName = "Dom Sportova", ScheduledAt = new DateTime(2026, 6, 19, 18, 0, 0), Status = "Completed", HomeScore = 28, AwayScore = 23 },
        new() { Id = 4, TournamentId = 1, HomeTeamId = 2, AwayTeamId = 3, HomeTeamName = "RK Nexe", AwayTeamName = "RK Split", RoundName = "Group A", VenueName = "Dom Sportova", ScheduledAt = new DateTime(2026, 6, 19, 20, 0, 0), Status = "Completed", HomeScore = 27, AwayScore = 26 },
        new() { Id = 5, TournamentId = 1, HomeTeamId = 1, AwayTeamId = 2, HomeTeamName = "RK Zagreb", AwayTeamName = "RK Nexe", RoundName = "Group A", VenueName = "Arena Zagreb", ScheduledAt = new DateTime(2026, 6, 20, 19, 0, 0), Status = "Scheduled" },
        new() { Id = 6, TournamentId = 1, HomeTeamId = 3, AwayTeamId = 4, HomeTeamName = "RK Split", AwayTeamName = "RK Varazdin", RoundName = "Group A", VenueName = "Arena Zagreb", ScheduledAt = new DateTime(2026, 6, 20, 21, 0, 0), Status = "Scheduled" },
        new() { Id = 7, TournamentId = 1, HomeTeamId = 21, AwayTeamId = 23, HomeTeamName = "RK Porec", AwayTeamName = "RK Metkovic", RoundName = "Group B", VenueName = "Sutinska Vrela", ScheduledAt = new DateTime(2026, 6, 18, 16, 0, 0), Status = "Completed", HomeScore = 30, AwayScore = 27 },
        new() { Id = 8, TournamentId = 1, HomeTeamId = 22, AwayTeamId = 24, HomeTeamName = "RK Sesvete", AwayTeamName = "RK Bjelovar", RoundName = "Group B", VenueName = "Sutinska Vrela", ScheduledAt = new DateTime(2026, 6, 18, 18, 0, 0), Status = "Completed", HomeScore = 26, AwayScore = 24 },
        new() { Id = 11, TournamentId = 1, HomeTeamId = 21, AwayTeamId = 24, HomeTeamName = "RK Porec", AwayTeamName = "RK Bjelovar", RoundName = "Group B", VenueName = "Arena Zagreb", ScheduledAt = new DateTime(2026, 6, 19, 16, 0, 0), Status = "Completed", HomeScore = 29, AwayScore = 29 },
        new() { Id = 12, TournamentId = 1, HomeTeamId = 22, AwayTeamId = 23, HomeTeamName = "RK Sesvete", AwayTeamName = "RK Metkovic", RoundName = "Group B", VenueName = "Arena Zagreb", ScheduledAt = new DateTime(2026, 6, 19, 18, 0, 0), Status = "Completed", HomeScore = 28, AwayScore = 25 },
        new() { Id = 13, TournamentId = 1, HomeTeamId = 21, AwayTeamId = 22, HomeTeamName = "RK Porec", AwayTeamName = "RK Sesvete", RoundName = "Group B", VenueName = "Dom Sportova", ScheduledAt = new DateTime(2026, 6, 20, 17, 0, 0), Status = "Scheduled" },
        new() { Id = 14, TournamentId = 1, HomeTeamId = 23, AwayTeamId = 24, HomeTeamName = "RK Metkovic", AwayTeamName = "RK Bjelovar", RoundName = "Group B", VenueName = "Dom Sportova", ScheduledAt = new DateTime(2026, 6, 20, 19, 0, 0), Status = "Scheduled" },
        new() { Id = 15, TournamentId = 1, HomeTeamId = 0, AwayTeamId = 0, HomeTeamName = "A1", AwayTeamName = "B2", RoundName = "Semifinal", VenueName = "Arena Zagreb", ScheduledAt = new DateTime(2026, 6, 21, 18, 0, 0), Status = "Scheduled" },
        new() { Id = 16, TournamentId = 1, HomeTeamId = 0, AwayTeamId = 0, HomeTeamName = "A2", AwayTeamName = "B1", RoundName = "Semifinal", VenueName = "Arena Zagreb", ScheduledAt = new DateTime(2026, 6, 21, 20, 0, 0), Status = "Scheduled" },
        new() { Id = 17, TournamentId = 1, HomeTeamId = 0, AwayTeamId = 0, HomeTeamName = "Winner SF1", AwayTeamName = "Winner SF2", RoundName = "Final", VenueName = "Arena Zagreb", ScheduledAt = new DateTime(2026, 6, 22, 20, 0, 0), Status = "Scheduled" },
        new() { Id = 9, TournamentId = 2, HomeTeamId = 5, AwayTeamId = 6, HomeTeamName = "RK Zadar", AwayTeamName = "RK Dubrovnik", RoundName = "Group A", VenueName = "Spaladium Arena", ScheduledAt = new DateTime(2026, 7, 8, 17, 0, 0), Status = "Scheduled" },
        new() { Id = 10, TournamentId = 3, HomeTeamId = 7, AwayTeamId = 4, HomeTeamName = "RK Celje", AwayTeamName = "RK Varazdin", RoundName = "Final", VenueName = "Varazdin Arena", ScheduledAt = new DateTime(2026, 5, 16, 19, 30, 0), Status = "Completed", HomeScore = 30, AwayScore = 27 }
    ];

    internal static readonly IReadOnlyList<MatchEventSummaryViewModel> MatchEvents =
    [
        new() { Id = 1, MatchId = 1, PlayerId = 1, TeamId = 1, PlayerName = "Luka Vidovic", TeamName = "RK Zagreb", EventType = "Goal", Minute = 3 },
        new() { Id = 2, MatchId = 1, PlayerId = 7, TeamId = 3, PlayerName = "Duje Matic", TeamName = "RK Split", EventType = "Goal", Minute = 5 },
        new() { Id = 3, MatchId = 1, PlayerId = 2, TeamId = 1, PlayerName = "Mateo Kolar", TeamName = "RK Zagreb", EventType = "Yellow Card", Minute = 14 },
        new() { Id = 4, MatchId = 1, PlayerId = 8, TeamId = 3, PlayerName = "Marin Grgic", TeamName = "RK Split", EventType = "Two-Minute Suspension", Minute = 42 },
        new() { Id = 9, MatchId = 1, PlayerId = 1, TeamId = 1, PlayerName = "Luka Vidovic", TeamName = "RK Zagreb", EventType = "Goal", Minute = 17 },
        new() { Id = 10, MatchId = 1, PlayerId = 8, TeamId = 3, PlayerName = "Marin Grgic", TeamName = "RK Split", EventType = "Goal", Minute = 21 },
        new() { Id = 11, MatchId = 1, PlayerId = 2, TeamId = 1, PlayerName = "Mateo Kolar", TeamName = "RK Zagreb", EventType = "Goal", Minute = 29 },
        new() { Id = 12, MatchId = 1, PlayerId = 7, TeamId = 3, PlayerName = "Duje Matic", TeamName = "RK Split", EventType = "Goal", Minute = 36 },
        new() { Id = 13, MatchId = 1, PlayerId = 1, TeamId = 1, PlayerName = "Luka Vidovic", TeamName = "RK Zagreb", EventType = "Two-Minute Suspension", Minute = 48 },
        new() { Id = 14, MatchId = 1, PlayerId = 8, TeamId = 3, PlayerName = "Marin Grgic", TeamName = "RK Split", EventType = "Goal", Minute = 54 },
        new() { Id = 5, MatchId = 2, PlayerId = 4, TeamId = 2, PlayerName = "Ivan Radic", TeamName = "RK Nexe", EventType = "Goal", Minute = 8 },
        new() { Id = 6, MatchId = 2, PlayerId = 10, TeamId = 4, PlayerName = "Leon Novak", TeamName = "RK Varazdin", EventType = "Goal", Minute = 11 },
        new() { Id = 15, MatchId = 2, PlayerId = 5, TeamId = 2, PlayerName = "Tomislav Bencic", TeamName = "RK Nexe", EventType = "Yellow Card", Minute = 18 },
        new() { Id = 16, MatchId = 2, PlayerId = 11, TeamId = 4, PlayerName = "Karlo Horvat", TeamName = "RK Varazdin", EventType = "Two-Minute Suspension", Minute = 33 },
        new() { Id = 17, MatchId = 2, PlayerId = 4, TeamId = 2, PlayerName = "Ivan Radic", TeamName = "RK Nexe", EventType = "Goal", Minute = 49 },
        new() { Id = 18, MatchId = 2, PlayerId = 10, TeamId = 4, PlayerName = "Leon Novak", TeamName = "RK Varazdin", EventType = "Yellow Card", Minute = 57 },
        new() { Id = 7, MatchId = 3, PlayerId = 1, TeamId = 1, PlayerName = "Luka Vidovic", TeamName = "RK Zagreb", EventType = "Goal", Minute = 27 },
        new() { Id = 8, MatchId = 4, PlayerId = 5, TeamId = 2, PlayerName = "Tomislav Bencic", TeamName = "RK Nexe", EventType = "Goal", Minute = 55 }
    ];

    internal static readonly IReadOnlyList<GroupStandingViewModel> GroupStandings =
    [
        new() { TournamentId = 1, TeamId = 1, GroupName = "Group A", TeamName = "RK Zagreb", Played = 2, Won = 2, Drawn = 0, Lost = 0, GoalsFor = 59, GoalsAgainst = 52, Points = 4 },
        new() { TournamentId = 1, TeamId = 2, GroupName = "Group A", TeamName = "RK Nexe", Played = 2, Won = 1, Drawn = 1, Lost = 0, GoalsFor = 52, GoalsAgainst = 51, Points = 3 },
        new() { TournamentId = 1, TeamId = 4, GroupName = "Group A", TeamName = "RK Varazdin", Played = 2, Won = 0, Drawn = 1, Lost = 1, GoalsFor = 48, GoalsAgainst = 53, Points = 1 },
        new() { TournamentId = 1, TeamId = 3, GroupName = "Group A", TeamName = "RK Split", Played = 2, Won = 0, Drawn = 0, Lost = 2, GoalsFor = 55, GoalsAgainst = 58, Points = 0 },
        new() { TournamentId = 1, TeamId = 22, GroupName = "Group B", TeamName = "RK Sesvete", Played = 2, Won = 2, Drawn = 0, Lost = 0, GoalsFor = 54, GoalsAgainst = 49, Points = 4 },
        new() { TournamentId = 1, TeamId = 21, GroupName = "Group B", TeamName = "RK Porec", Played = 2, Won = 1, Drawn = 1, Lost = 0, GoalsFor = 59, GoalsAgainst = 56, Points = 3 },
        new() { TournamentId = 1, TeamId = 24, GroupName = "Group B", TeamName = "RK Bjelovar", Played = 2, Won = 0, Drawn = 1, Lost = 1, GoalsFor = 53, GoalsAgainst = 55, Points = 1 },
        new() { TournamentId = 1, TeamId = 23, GroupName = "Group B", TeamName = "RK Metkovic", Played = 2, Won = 0, Drawn = 0, Lost = 2, GoalsFor = 52, GoalsAgainst = 58, Points = 0 }
    ];

    internal static readonly IReadOnlyList<PlayerStatisticViewModel> PlayerStatistics =
    [
        new() { TournamentId = 1, PlayerId = 1, TeamId = 1, PlayerName = "Luka Vidovic", TeamName = "RK Zagreb", Goals = 15, YellowCards = 0, RedCards = 0, TwoMinuteSuspensions = 1 },
        new() { TournamentId = 1, PlayerId = 2, TeamId = 1, PlayerName = "Mateo Kolar", TeamName = "RK Zagreb", Goals = 9, YellowCards = 1, RedCards = 0, TwoMinuteSuspensions = 0 },
        new() { TournamentId = 1, PlayerId = 4, TeamId = 2, PlayerName = "Ivan Radic", TeamName = "RK Nexe", Goals = 12, YellowCards = 0, RedCards = 0, TwoMinuteSuspensions = 1 },
        new() { TournamentId = 1, PlayerId = 5, TeamId = 2, PlayerName = "Tomislav Bencic", TeamName = "RK Nexe", Goals = 8, YellowCards = 1, RedCards = 0, TwoMinuteSuspensions = 2 },
        new() { TournamentId = 1, PlayerId = 7, TeamId = 3, PlayerName = "Duje Matic", TeamName = "RK Split", Goals = 11, YellowCards = 0, RedCards = 0, TwoMinuteSuspensions = 0 },
        new() { TournamentId = 1, PlayerId = 8, TeamId = 3, PlayerName = "Marin Grgic", TeamName = "RK Split", Goals = 10, YellowCards = 0, RedCards = 0, TwoMinuteSuspensions = 1 },
        new() { TournamentId = 1, PlayerId = 10, TeamId = 4, PlayerName = "Leon Novak", TeamName = "RK Varazdin", Goals = 13, YellowCards = 1, RedCards = 0, TwoMinuteSuspensions = 1 },
        new() { TournamentId = 1, PlayerId = 11, TeamId = 4, PlayerName = "Karlo Horvat", TeamName = "RK Varazdin", Goals = 7, YellowCards = 0, RedCards = 0, TwoMinuteSuspensions = 2 },
        new() { TournamentId = 1, PlayerId = 21, TeamId = 21, PlayerName = "Lovro Krizan", TeamName = "RK Porec", Goals = 14, YellowCards = 0, RedCards = 0, TwoMinuteSuspensions = 1 },
        new() { TournamentId = 1, PlayerId = 22, TeamId = 21, PlayerName = "Noa Matosevic", TeamName = "RK Porec", Goals = 2, YellowCards = 0, RedCards = 0, TwoMinuteSuspensions = 0 },
        new() { TournamentId = 1, PlayerId = 23, TeamId = 22, PlayerName = "Patrik Vidak", TeamName = "RK Sesvete", Goals = 13, YellowCards = 1, RedCards = 0, TwoMinuteSuspensions = 1 },
        new() { TournamentId = 1, PlayerId = 24, TeamId = 22, PlayerName = "Emanuel Grbic", TeamName = "RK Sesvete", Goals = 8, YellowCards = 0, RedCards = 0, TwoMinuteSuspensions = 0 },
        new() { TournamentId = 1, PlayerId = 25, TeamId = 23, PlayerName = "Marin Vukovic", TeamName = "RK Metkovic", Goals = 12, YellowCards = 1, RedCards = 0, TwoMinuteSuspensions = 2 },
        new() { TournamentId = 1, PlayerId = 26, TeamId = 23, PlayerName = "Kresimir Bago", TeamName = "RK Metkovic", Goals = 7, YellowCards = 0, RedCards = 1, TwoMinuteSuspensions = 1 },
        new() { TournamentId = 1, PlayerId = 27, TeamId = 24, PlayerName = "Tin Markovic", TeamName = "RK Bjelovar", Goals = 10, YellowCards = 0, RedCards = 0, TwoMinuteSuspensions = 1 },
        new() { TournamentId = 1, PlayerId = 28, TeamId = 24, PlayerName = "Bruno Cacic", TeamName = "RK Bjelovar", Goals = 9, YellowCards = 1, RedCards = 0, TwoMinuteSuspensions = 0 }
    ];
}
