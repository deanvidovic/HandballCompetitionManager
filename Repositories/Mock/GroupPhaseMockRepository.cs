namespace HandballCompetitionManager.Repositories.Mock;

using HandballCompetitionManager.Models;

public class GroupPhaseMockRepository
{
    private readonly TeamMockRepository _teamRepository;
    private readonly MatchMockRepository _matchRepository;
    private List<GroupPhase> _groups;

    public GroupPhaseMockRepository(TeamMockRepository teamRepository = null, MatchMockRepository matchRepository = null)
    {
        _teamRepository = teamRepository ?? new TeamMockRepository();
        _matchRepository = matchRepository ?? new MatchMockRepository();
        InitializeGroups();
    }

    private void InitializeGroups()
    {
        var teamRepo = new TeamMockRepository();
        var matchRepo = new MatchMockRepository();

        _groups = new()
        {
            new GroupPhase
            {
                Id = 1,
                Name = "Group A",
                CompetitionId = 1,
                Teams = new List<Team> { teamRepo.GetById(1), teamRepo.GetById(2) }.Where(t => t != null).ToList(),
                Matches = matchRepo.GetByGroupId(1)
            },
            new GroupPhase
            {
                Id = 2,
                Name = "Group B",
                CompetitionId = 1,
                Teams = new List<Team> { teamRepo.GetById(3), teamRepo.GetById(4) }.Where(t => t != null).ToList(),
                Matches = matchRepo.GetByGroupId(2)
            },
            new GroupPhase
            {
                Id = 3,
                Name = "Group C",
                CompetitionId = 1,
                Teams = new List<Team> { teamRepo.GetById(5) }.Where(t => t != null).ToList(),
                Matches = matchRepo.GetByGroupId(3)
            },
            new GroupPhase
            {
                Id = 4,
                Name = "Knockout Stage",
                CompetitionId = 1,
                Teams = new List<Team> { teamRepo.GetById(1), teamRepo.GetById(3) }.Where(t => t != null).ToList(),
                Matches = matchRepo.GetByGroupId(4)
            },
            new GroupPhase
            {
                Id = 5,
                Name = "Group A",
                CompetitionId = 2,
                Teams = new List<Team> { teamRepo.GetById(1), teamRepo.GetById(2), teamRepo.GetById(3) }.Where(t => t != null).ToList(),
                Matches = matchRepo.GetByGroupId(5)
            },
            new GroupPhase
            {
                Id = 6,
                Name = "Group B",
                CompetitionId = 2,
                Teams = new List<Team> { teamRepo.GetById(4), teamRepo.GetById(5) }.Where(t => t != null).ToList(),
                Matches = matchRepo.GetByGroupId(6)
            },
            new GroupPhase
            {
                Id = 7,
                Name = "Final Four",
                CompetitionId = 2,
                Teams = new List<Team> { teamRepo.GetById(1), teamRepo.GetById(2) }.Where(t => t != null).ToList(),
                Matches = matchRepo.GetByGroupId(7)
            },
            new GroupPhase
            {
                Id = 8,
                Name = "Main Round",
                CompetitionId = 3,
                Teams = new List<Team> { teamRepo.GetById(2), teamRepo.GetById(3), teamRepo.GetById(4) }.Where(t => t != null).ToList(),
                Matches = matchRepo.GetByGroupId(8)
            }
        };
    }

    public List<GroupPhase> GetAll()
    {
        return _groups ?? new List<GroupPhase>();
    }

    public GroupPhase? GetById(int id)
    {
        return GetAll().FirstOrDefault(g => g.Id == id);
    }

    public List<GroupPhase> GetByCompetitionId(int competitionId)
    {
        return GetAll().Where(g => g.CompetitionId == competitionId).ToList();
    }

    public int GetTeamCountInGroup(int groupId)
    {
        var group = GetById(groupId);
        return group?.Teams.Count ?? 0;
    }

    public int GetMatchCountInGroup(int groupId)
    {
        var group = GetById(groupId);
        return group?.Matches.Count ?? 0;
    }
}
