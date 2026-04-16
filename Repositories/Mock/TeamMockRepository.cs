namespace HandballCompetitionManager.Repositories.Mock;

using HandballCompetitionManager.Models;

public class TeamMockRepository
{
    private List<Team> _teams;

    public TeamMockRepository()
    {
        InitializeTeams();
    }

    private void InitializeTeams()
    {
        var playerRepo = new PlayerMockRepository();
        
        _teams = new()
        {
            new Team
            {
                Id = 1,
                Name = "Zagreb Tigers",
                Club = "HK Zagreb",
                CoachName = "Ivan Horvat",
                HomeCity = "Zagreb",
                FoundedYear = 2010,
                HomeArena = "Pabellon Mladost",
                Players = playerRepo.GetByTeamId(1),
                Competitions = new()
            },
            new Team
            {
                Id = 2,
                Name = "Rijeka Hawks",
                Club = "HK Rijeka",
                CoachName = "Marko Milic",
                HomeCity = "Rijeka",
                FoundedYear = 2005,
                HomeArena = "Gradski vrt",
                Players = playerRepo.GetByTeamId(2),
                Competitions = new()
            },
            new Team
            {
                Id = 3,
                Name = "Split Phoenix",
                Club = "HK Split",
                CoachName = "Ante Antic",
                HomeCity = "Split",
                FoundedYear = 2008,
                HomeArena = "Poljud Arena",
                Players = playerRepo.GetByTeamId(3),
                Competitions = new()
            },
            new Team
            {
                Id = 4,
                Name = "Osijek Wolves",
                Club = "HK Osijek",
                CoachName = "Drago Horvat",
                HomeCity = "Osijek",
                FoundedYear = 2012,
                HomeArena = "Gradski vrt",
                Players = new(),
                Competitions = new()
            },
            new Team
            {
                Id = 5,
                Name = "Zadar Dolphins",
                Club = "HK Zadar",
                CoachName = "Jure Horvat",
                HomeCity = "Zadar",
                FoundedYear = 2009,
                HomeArena = "Zaro",
                Players = new(),
                Competitions = new()
            }
        };
    }

    public List<Team> GetAll()
    {
        return _teams ?? new List<Team>();
    }

    public Team? GetById(int id)
    {
        return GetAll().FirstOrDefault(t => t.Id == id);
    }

    public List<Team> GetByCity(string city)
    {
        return GetAll().Where(t => t.HomeCity == city).ToList();
    }
}
