namespace HandballCompetitionManager.Repositories.Mock;

using HandballCompetitionManager.Models;

public class PlayerMockRepository
{
    // Zagreb Tigers players (Id 1-7)
    private static readonly List<Player> _players = new()
    {
        new Player
        {
            Id = 1,
            FirstName = "Luka",
            LastName = "Kalinić",
            BirthDate = new DateTime(1992, 5, 15),
            JerseyNumber = 1,
            Position = PlayerPosition.Goalkeeper,
            TeamId = 1,
            GoalsScored = 0,
            Assists = 0
        },
        new Player
        {
            Id = 2,
            FirstName = "Domagoj",
            LastName = "Duvnjak",
            BirthDate = new DateTime(1988, 3, 25),
            JerseyNumber = 9,
            Position = PlayerPosition.RightWing,
            TeamId = 1,
            GoalsScored = 145,
            Assists = 28
        },
        new Player
        {
            Id = 3,
            FirstName = "Marko",
            LastName = "Mamić",
            BirthDate = new DateTime(1995, 7, 10),
            JerseyNumber = 8,
            Position = PlayerPosition.LeftWing,
            TeamId = 1,
            GoalsScored = 98,
            Assists = 15
        },
        new Player
        {
            Id = 4,
            FirstName = "Šarić",
            LastName = "Tomislav",
            BirthDate = new DateTime(1990, 11, 22),
            JerseyNumber = 7,
            Position = PlayerPosition.Pivot,
            TeamId = 1,
            GoalsScored = 76,
            Assists = 42
        },
        new Player
        {
            Id = 5,
            FirstName = "Ivan",
            LastName = "Čupić",
            BirthDate = new DateTime(1994, 2, 8),
            JerseyNumber = 3,
            Position = PlayerPosition.LeftBack,
            TeamId = 1,
            GoalsScored = 54,
            Assists = 22
        },
        new Player
        {
            Id = 6,
            FirstName = "Toni",
            LastName = "Popadic",
            BirthDate = new DateTime(1993, 9, 14),
            JerseyNumber = 6,
            Position = PlayerPosition.RightBack,
            TeamId = 1,
            GoalsScored = 67,
            Assists = 18
        },
        new Player
        {
            Id = 7,
            FirstName = "Marino",
            LastName = "Marić",
            BirthDate = new DateTime(1991, 6, 19),
            JerseyNumber = 5,
            Position = PlayerPosition.CenterBack,
            TeamId = 1,
            GoalsScored = 45,
            Assists = 12
        },
        // Rijeka Hawks players (Id 8-14)
        new Player
        {
            Id = 8,
            FirstName = "Marinović",
            LastName = "Andrija",
            BirthDate = new DateTime(1996, 8, 3),
            JerseyNumber = 1,
            Position = PlayerPosition.Goalkeeper,
            TeamId = 2,
            GoalsScored = 0,
            Assists = 0
        },
        new Player
        {
            Id = 9,
            FirstName = "Penava",
            LastName = "Marko",
            BirthDate = new DateTime(1989, 4, 17),
            JerseyNumber = 11,
            Position = PlayerPosition.LeftWing,
            TeamId = 2,
            GoalsScored = 112,
            Assists = 31
        },
        new Player
        {
            Id = 10,
            FirstName = "Jelovčić",
            LastName = "Petar",
            BirthDate = new DateTime(1991, 12, 28),
            JerseyNumber = 9,
            Position = PlayerPosition.RightWing,
            TeamId = 2,
            GoalsScored = 88,
            Assists = 19
        },
        new Player
        {
            Id = 11,
            FirstName = "Horvat",
            LastName = "Milan",
            BirthDate = new DateTime(1993, 1, 11),
            JerseyNumber = 7,
            Position = PlayerPosition.Pivot,
            TeamId = 2,
            GoalsScored = 73,
            Assists = 38
        },
        new Player
        {
            Id = 12,
            FirstName = "Kos",
            LastName = "Damir",
            BirthDate = new DateTime(1994, 7, 5),
            JerseyNumber = 2,
            Position = PlayerPosition.LeftBack,
            TeamId = 2,
            GoalsScored = 51,
            Assists = 14
        },
        new Player
        {
            Id = 13,
            FirstName = "Marić",
            LastName = "Goran",
            BirthDate = new DateTime(1992, 10, 9),
            JerseyNumber = 4,
            Position = PlayerPosition.CenterBack,
            TeamId = 2,
            GoalsScored = 39,
            Assists = 8
        },
        new Player
        {
            Id = 14,
            FirstName = "Bošnjak",
            LastName = "Igor",
            BirthDate = new DateTime(1995, 3, 21),
            JerseyNumber = 6,
            Position = PlayerPosition.RightBack,
            TeamId = 2,
            GoalsScored = 44,
            Assists = 11
        },
        // Split Phoenix players (Id 15-21)
        new Player
        {
            Id = 15,
            FirstName = "Perović",
            LastName = "Darko",
            BirthDate = new DateTime(1997, 6, 14),
            JerseyNumber = 1,
            Position = PlayerPosition.Goalkeeper,
            TeamId = 3,
            GoalsScored = 0,
            Assists = 0
        },
        new Player
        {
            Id = 16,
            FirstName = "Bulj",
            LastName = "Zoran",
            BirthDate = new DateTime(1990, 9, 12),
            JerseyNumber = 10,
            Position = PlayerPosition.LeftWing,
            TeamId = 3,
            GoalsScored = 105,
            Assists = 25
        },
        new Player
        {
            Id = 17,
            FirstName = "Špoljar",
            LastName = "Veljko",
            BirthDate = new DateTime(1992, 2, 8),
            JerseyNumber = 9,
            Position = PlayerPosition.RightWing,
            TeamId = 3,
            GoalsScored = 92,
            Assists = 21
        },
        new Player
        {
            Id = 18,
            FirstName = "Sladković",
            LastName = "Jure",
            BirthDate = new DateTime(1994, 5, 19),
            JerseyNumber = 7,
            Position = PlayerPosition.Pivot,
            TeamId = 3,
            GoalsScored = 81,
            Assists = 40
        }
    };

    public List<Player> GetAll()
    {
        return _players;
    }

    public Player? GetById(int id)
    {
        return _players.FirstOrDefault(p => p.Id == id);
    }

    public List<Player> GetByTeamId(int teamId)
    {
        return _players.Where(p => p.TeamId == teamId).ToList();
    }

    public List<Player> GetByPosition(PlayerPosition position)
    {
        return _players.Where(p => p.Position == position).ToList();
    }
}
