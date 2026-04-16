namespace HandballCompetitionManager.Repositories.Mock;

using HandballCompetitionManager.Models;

public class CompetitionMockRepository
{
    private readonly TeamMockRepository _teamRepository;
    private readonly GroupPhaseMockRepository _groupRepository;
    private List<Competition> _competitions = new();

    public CompetitionMockRepository(TeamMockRepository? teamRepository = null, GroupPhaseMockRepository? groupRepository = null)
    {
        _teamRepository = teamRepository ?? new TeamMockRepository();
        _groupRepository = groupRepository ?? new GroupPhaseMockRepository();
        InitializeCompetitions();
    }

    private void InitializeCompetitions()
    {
        var teamRepo = new TeamMockRepository();
        var groupRepo = new GroupPhaseMockRepository();

        _competitions = new()
        {
            new Competition
            {
                Id = 1,
                Name = "Croatian Handball Cup 2026",
                Season = "2025/2026",
                StartDate = new DateTime(2026, 2, 1),
                EndDate = new DateTime(2026, 5, 15),
                City = "Zagreb",
                Teams = new List<Team> { teamRepo.GetById(1), teamRepo.GetById(2), teamRepo.GetById(3), teamRepo.GetById(4), teamRepo.GetById(5) }.Where(t => t != null).ToList(),
                Groups = groupRepo.GetByCompetitionId(1),
                Administrators = new()
            },
            new Competition
            {
                Id = 2,
                Name = "Adriatic League 2026",
                Season = "2025/2026",
                StartDate = new DateTime(2026, 1, 15),
                EndDate = new DateTime(2026, 6, 30),
                City = "Zagreb",
                Teams = new List<Team> { teamRepo.GetById(1), teamRepo.GetById(2), teamRepo.GetById(3), teamRepo.GetById(4) }.Where(t => t != null).ToList(),
                Groups = groupRepo.GetByCompetitionId(2),
                Administrators = new()
            },
            new Competition
            {
                Id = 3,
                Name = "Regional Championship Split",
                Season = "2025/2026",
                StartDate = new DateTime(2026, 3, 1),
                EndDate = new DateTime(2026, 4, 30),
                City = "Split",
                Teams = new List<Team> { teamRepo.GetById(2), teamRepo.GetById(3), teamRepo.GetById(4) }.Where(t => t != null).ToList(),
                Groups = groupRepo.GetByCompetitionId(3),
                Administrators = new()
            },
            new Competition
            {
                Id = 4,
                Name = "Youth Cup 2026",
                Season = "2025/2026",
                StartDate = new DateTime(2026, 4, 1),
                EndDate = new DateTime(2026, 5, 20),
                City = "Rijeka",
                Teams = new List<Team> { teamRepo.GetById(1), teamRepo.GetById(5) }.Where(t => t != null).ToList(),
                Groups = groupRepo.GetByCompetitionId(4),
                Administrators = new()
            }
        };
    }

    public List<Competition> GetAll()
    {
        return _competitions ?? new List<Competition>();
    }

    public Competition? GetById(int id)
    {
        return GetAll().FirstOrDefault(c => c.Id == id);
    }

    public List<Competition> GetByseason(string season)
    {
        return GetAll().Where(c => c.Season == season).ToList();
    }

    public List<Competition> GetByCity(string city)
    {
        return GetAll().Where(c => c.City == city).ToList();
    }

    public List<Competition> GetActive()
    {
        var now = DateTime.Now;
        return GetAll().Where(c => c.StartDate <= now && c.EndDate >= now).ToList();
    }
}
