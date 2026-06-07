using HandballCompetitionManager.Models;
using HandballCompetitionManager.Data;
using Microsoft.EntityFrameworkCore;

namespace HandballCompetitionManager.Repositories;

public class TeamRepository
{
    private readonly HandballDbContext _context;

    public TeamRepository(HandballDbContext context)
    {
        _context = context;
    }

    public List<Team> GetAll()
    {
        return _context.Teams
            .Where(t => t.DeletedAt == null)
            .Include(t => t.Players.Where(p => p.DeletedAt == null))
            .Include(t => t.Competitions.Where(c => c.DeletedAt == null))
            .ToList();
    }

    public List<Team> Search(string? query)
    {
        var teams = _context.Teams
            .Where(t => t.DeletedAt == null)
            .Include(t => t.Players.Where(p => p.DeletedAt == null))
            .Include(t => t.Competitions.Where(c => c.DeletedAt == null))
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var term = $"%{query.Trim()}%";
            teams = teams.Where(t =>
                EF.Functions.Like(t.Name, term) ||
                EF.Functions.Like(t.CoachName, term) ||
                EF.Functions.Like(t.HomeCity, term) ||
                EF.Functions.Like(t.HomeArena, term));
        }

        return teams.ToList();
    }

    public Team? GetById(int id)
    {
        return _context.Teams
            .Where(t => t.DeletedAt == null)
            .Include(t => t.Players.Where(p => p.DeletedAt == null))
            .Include(t => t.Competitions.Where(c => c.DeletedAt == null))
            .FirstOrDefault(t => t.Id == id);
    }

    public List<Team> GetAvailableForCompetition(DateTime startDate, DateTime endDate, int competitionId)
    {
        return _context.Teams
            .Where(t => t.DeletedAt == null)
            .Include(t => t.Players.Where(p => p.DeletedAt == null))
            .Include(t => t.Competitions.Where(c => c.DeletedAt == null))
            .Where(t => !t.Competitions.Any(c =>
                c.DeletedAt == null &&
                c.Id != competitionId &&
                c.StartDate.Date <= endDate.Date &&
                c.EndDate.Date >= startDate.Date))
            .OrderBy(t => t.Name)
            .ToList();
    }

    public List<Team> GetByCity(string city)
    {
        return _context.Teams.Where(t => t.DeletedAt == null && t.HomeCity == city).ToList();
    }

    public void Add(Team team)
    {
        _context.Teams.Add(team);
        _context.SaveChanges();
    }

    public void Update(Team team)
    {
        _context.Teams.Update(team);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var team = GetById(id);
        if (team != null)
        {
            var deletedAt = DateTime.UtcNow;
            team.DeletedAt = deletedAt;

            foreach (var player in team.Players.Where(player => player.DeletedAt == null))
            {
                player.DeletedAt = deletedAt;
            }

            _context.SaveChanges();
        }
    }
}
