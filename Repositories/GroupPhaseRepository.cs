using HandballCompetitionManager.Models;
using HandballCompetitionManager.Data;
using Microsoft.EntityFrameworkCore;

namespace HandballCompetitionManager.Repositories;

public class GroupPhaseRepository
{
    private readonly HandballDbContext _context;

    public GroupPhaseRepository(HandballDbContext context)
    {
        _context = context;
    }

    public List<GroupPhase> GetAll()
    {
        return _context.GroupPhases
            .Where(g => g.Competition != null && g.Competition.DeletedAt == null)
            .Include(g => g.Competition)
            .Include(g => g.Teams.Where(t => t.DeletedAt == null))
            .Include(g => g.Matches)
            .ToList();
    }

    public GroupPhase? GetById(int id)
    {
        return _context.GroupPhases
            .Where(g => g.Competition != null && g.Competition.DeletedAt == null)
            .Include(g => g.Competition)
            .Include(g => g.Teams.Where(t => t.DeletedAt == null))
            .Include(g => g.Matches)
            .FirstOrDefault(g => g.Id == id);
    }

    public List<GroupPhase> GetByCompetitionId(int competitionId)
    {
        return _context.GroupPhases
            .Where(g => g.CompetitionId == competitionId && g.Competition != null && g.Competition.DeletedAt == null)
            .Include(g => g.Teams.Where(t => t.DeletedAt == null))
            .Include(g => g.Matches)
            .ToList();
    }

    public void Add(GroupPhase groupPhase)
    {
        _context.GroupPhases.Add(groupPhase);
        _context.SaveChanges();
    }

    public void Update(GroupPhase groupPhase)
    {
        _context.GroupPhases.Update(groupPhase);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var groupPhase = GetById(id);
        if (groupPhase != null)
        {
            _context.GroupPhases.Remove(groupPhase);
            _context.SaveChanges();
        }
    }
}
