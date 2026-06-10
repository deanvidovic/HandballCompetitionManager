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
        return BuildGroupPhaseQuery().ToList();
    }

    public List<GroupPhase> GetAllForUser(int userId)
    {
        return BuildGroupPhaseQuery()
            .Where(g =>
                g.Competition != null &&
                g.Competition.Administrators.Any(a => a.Id == userId))
            .ToList();
    }

    public GroupPhase? GetById(int id)
    {
        return BuildGroupPhaseQuery().FirstOrDefault(g => g.Id == id);
    }

    public GroupPhase? GetByIdForUser(int id, int userId)
    {
        return BuildGroupPhaseQuery()
            .FirstOrDefault(g =>
                g.Id == id &&
                g.Competition != null &&
                g.Competition.Administrators.Any(a => a.Id == userId));
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

    private IQueryable<GroupPhase> BuildGroupPhaseQuery()
    {
        return _context.GroupPhases
            .Where(g => g.Competition != null && g.Competition.DeletedAt == null)
            .Include(g => g.Competition)
                .ThenInclude(c => c!.Administrators)
            .Include(g => g.Teams.Where(t => t.DeletedAt == null))
            .Include(g => g.Matches);
    }
}
