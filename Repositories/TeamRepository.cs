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
        var teams = BuildTeamQuery();

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

    public List<Team> SearchForUser(string? query, int userId)
    {
        var teams = BuildTeamQuery()
            .Where(t => t.Competitions.Any(c =>
                c.DeletedAt == null &&
                c.Administrators.Any(a => a.Id == userId)));

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

    public List<Team> SearchForCoach(string? query, string coachName)
    {
        var teams = BuildTeamQuery()
            .Where(t => t.CoachName == coachName);

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
        return BuildTeamQuery()
            .FirstOrDefault(t => t.Id == id);
    }

    public Team? GetByIdForUser(int id, int userId)
    {
        return BuildTeamQuery()
            .FirstOrDefault(t =>
                t.Id == id &&
                t.Competitions.Any(c =>
                    c.DeletedAt == null &&
                    c.Administrators.Any(a => a.Id == userId)));
    }

    public Team? GetByIdForCoach(int id, string coachName)
    {
        return BuildTeamQuery()
            .FirstOrDefault(t => t.Id == id && t.CoachName == coachName);
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
        return BuildTeamQuery().Where(t => t.HomeCity == city).ToList();
    }

    public List<Team> GetByCityForUser(string city, int userId)
    {
        return BuildTeamQuery()
            .Where(t =>
                t.HomeCity == city &&
                t.Competitions.Any(c =>
                    c.DeletedAt == null &&
                    c.Administrators.Any(a => a.Id == userId)))
            .ToList();
    }

    public List<Team> GetByCityForCoach(string city, string coachName)
    {
        return BuildTeamQuery()
            .Where(t => t.HomeCity == city && t.CoachName == coachName)
            .ToList();
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

    private IQueryable<Team> BuildTeamQuery()
    {
        return _context.Teams
            .Where(t => t.DeletedAt == null)
            .Include(t => t.Players.Where(p => p.DeletedAt == null))
            .Include(t => t.Competitions.Where(c => c.DeletedAt == null))
                .ThenInclude(c => c.Administrators);
    }
}
