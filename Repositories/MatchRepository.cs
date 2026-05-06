using HandballCompetitionManager.Models;
using HandballCompetitionManager.Data;
using Microsoft.EntityFrameworkCore;

namespace HandballCompetitionManager.Repositories;

public class MatchRepository
{
    private readonly HandballDbContext _context;

    public MatchRepository(HandballDbContext context)
    {
        _context = context;
    }

    public List<Match> GetAll()
    {
        return _context.Matches
            .Include(m => m.Competition)
            .Include(m => m.GroupPhase)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .ToList();
    }

    public Match? GetById(int id)
    {
        return _context.Matches
            .Include(m => m.Competition)
            .Include(m => m.GroupPhase)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .FirstOrDefault(m => m.Id == id);
    }

    public List<Match> GetByCompetitionId(int competitionId)
    {
        return _context.Matches
            .Where(m => m.CompetitionId == competitionId)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .ToList();
    }

    public List<Match> GetByGroupId(int groupId)
    {
        return _context.Matches
            .Where(m => m.GroupId == groupId)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .ToList();
    }

    public List<Match> GetByTeamId(int teamId)
    {
        return _context.Matches
            .Where(m => m.HomeTeamId == teamId || m.AwayTeamId == teamId)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .ToList();
    }

    public List<Match> GetByStatus(MatchStatus status)
    {
        return _context.Matches
            .Where(m => m.Status == status)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .ToList();
    }

    public void Add(Match match)
    {
        _context.Matches.Add(match);
        _context.SaveChanges();
    }

    public void Update(Match match)
    {
        _context.Matches.Update(match);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var match = GetById(id);
        if (match != null)
        {
            _context.Matches.Remove(match);
            _context.SaveChanges();
        }
    }
}
