using HandballCompetitionManager.Models;
using HandballCompetitionManager.Data;
using Microsoft.EntityFrameworkCore;

namespace HandballCompetitionManager.Repositories;

public class CompetitionRepository
{
    private readonly HandballDbContext _context;

    public CompetitionRepository(HandballDbContext context)
    {
        _context = context;
    }

    public List<Competition> GetAll()
    {
        return _context.Competitions
            .Where(c => c.DeletedAt == null)
            .Include(c => c.Teams.Where(t => t.DeletedAt == null))
            .Include(c => c.Groups)
            .Include(c => c.Administrators.Where(a => a.DeletedAt == null))
            .ToList();
    }

    public List<Competition> Search(string? query)
    {
        var competitions = _context.Competitions
            .Where(c => c.DeletedAt == null)
            .Include(c => c.Teams.Where(t => t.DeletedAt == null))
            .Include(c => c.Groups)
            .Include(c => c.Administrators.Where(a => a.DeletedAt == null))
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var term = $"%{query.Trim()}%";
            competitions = competitions.Where(c =>
                EF.Functions.Like(c.Name, term) ||
                EF.Functions.Like(c.City, term) ||
                EF.Functions.Like(c.Season, term));
        }

        return competitions.ToList();
    }

    public Competition? GetById(int id)
    {
        return _context.Competitions
            .Where(c => c.DeletedAt == null)
            .Include(c => c.Teams.Where(t => t.DeletedAt == null))
            .Include(c => c.Groups)
            .Include(c => c.Administrators.Where(a => a.DeletedAt == null))
            .FirstOrDefault(c => c.Id == id);
    }

    public List<Competition> GetByseason(string season)
    {
        return _context.Competitions.Where(c => c.DeletedAt == null && c.Season == season).ToList();
    }

    public List<Competition> GetByCity(string city)
    {
        return _context.Competitions.Where(c => c.DeletedAt == null && c.City == city).ToList();
    }

    public List<Competition> GetActive()
    {
        var now = DateTime.Now;
        return _context.Competitions.Where(c => c.DeletedAt == null && c.StartDate <= now && c.EndDate >= now).ToList();
    }

    public void Add(Competition competition)
    {
        _context.Competitions.Add(competition);
        _context.SaveChanges();
    }

    public bool AddTeam(int competitionId, int teamId)
    {
        var competition = _context.Competitions
            .Where(c => c.DeletedAt == null)
            .Include(c => c.Teams)
            .FirstOrDefault(c => c.Id == competitionId);
        var team = _context.Teams.FirstOrDefault(t => t.DeletedAt == null && t.Id == teamId);

        if (competition == null || team == null || competition.Teams.Any(t => t.Id == teamId))
        {
            return false;
        }

        competition.Teams.Add(team);
        _context.SaveChanges();
        return true;
    }

    public int AddTeams(int competitionId, IEnumerable<int> teamIds)
    {
        var competition = _context.Competitions
            .Where(c => c.DeletedAt == null)
            .Include(c => c.Teams)
            .FirstOrDefault(c => c.Id == competitionId);

        if (competition == null)
        {
            return 0;
        }

        var uniqueTeamIds = teamIds.Distinct().ToList();
        var existingTeamIds = competition.Teams.Select(team => team.Id).ToHashSet();
        var teams = _context.Teams
            .Where(team => team.DeletedAt == null && uniqueTeamIds.Contains(team.Id) && !existingTeamIds.Contains(team.Id))
            .ToList();

        foreach (var team in teams)
        {
            competition.Teams.Add(team);
        }

        _context.SaveChanges();
        return teams.Count;
    }

    public bool RemoveTeam(int competitionId, int teamId)
    {
        var competition = _context.Competitions
            .Where(c => c.DeletedAt == null)
            .Include(c => c.Teams)
            .FirstOrDefault(c => c.Id == competitionId);
        var team = competition?.Teams.FirstOrDefault(t => t.Id == teamId);

        if (competition == null || team == null)
        {
            return false;
        }

        competition.Teams.Remove(team);
        _context.SaveChanges();
        return true;
    }

    public (bool Success, string Message) GenerateBracket(int competitionId)
    {
        var competition = _context.Competitions
            .Where(c => c.DeletedAt == null)
            .Include(c => c.Teams)
            .Include(c => c.Groups)
                .ThenInclude(g => g.Teams)
            .Include(c => c.Groups)
                .ThenInclude(g => g.Matches)
            .FirstOrDefault(c => c.Id == competitionId);

        if (competition == null)
        {
            return (false, "Competition was not found.");
        }

        var teams = competition.Teams
            .Where(team => team.DeletedAt == null)
            .OrderBy(_ => Guid.NewGuid())
            .ToList();
        var teamCount = teams.Count;

        if (teamCount is not (6 or 8 or 12 or 16))
        {
            return (false, "Bracket generation currently supports 6, 8, 12, or 16 teams.");
        }

        var groupCount = teamCount <= 8 ? 2 : 4;
        var groupSize = teamCount / groupCount;

        if (competition.Groups.Count > 0)
        {
            var matches = competition.Groups.SelectMany(group => group.Matches).ToList();
            _context.Matches.RemoveRange(matches);
            _context.GroupPhases.RemoveRange(competition.Groups);
            _context.SaveChanges();
        }

        var kickoff = competition.StartDate.Date.AddHours(18);
        var matchIndex = 0;

        for (var groupIndex = 0; groupIndex < groupCount; groupIndex += 1)
        {
            var groupTeams = teams.Skip(groupIndex * groupSize).Take(groupSize).ToList();
            var group = new GroupPhase
            {
                Name = $"Group {(char)('A' + groupIndex)}",
                CompetitionId = competition.Id
            };

            foreach (var team in groupTeams)
            {
                group.Teams.Add(team);
            }

            for (var homeIndex = 0; homeIndex < groupTeams.Count; homeIndex += 1)
            {
                for (var awayIndex = homeIndex + 1; awayIndex < groupTeams.Count; awayIndex += 1)
                {
                    group.Matches.Add(new Match
                    {
                        CompetitionId = competition.Id,
                        RoundNumber = groupIndex + 1,
                        Kickoff = kickoff.AddDays(matchIndex),
                        HomeTeamId = groupTeams[homeIndex].Id,
                        AwayTeamId = groupTeams[awayIndex].Id,
                        HomeScore = 0,
                        AwayScore = 0,
                        MaintenanceHall = competition.City,
                        Status = MatchStatus.Scheduled
                    });
                    matchIndex += 1;
                }
            }

            _context.GroupPhases.Add(group);
        }

        _context.SaveChanges();

        var knockoutStart = groupCount == 2 ? "semifinals" : "quarter finals";
        return (true, $"Competition bracket generated: {groupCount} groups of {groupSize}. Top 2 teams from each group advance to {knockoutStart}.");
    }

    public void Update(Competition competition)
    {
        _context.Competitions.Update(competition);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var competition = GetById(id);
        if (competition != null)
        {
            competition.DeletedAt = DateTime.UtcNow;
            _context.SaveChanges();
        }
    }
}
