using HandballCompetitionManager.Models;
using HandballCompetitionManager.Data;
using Microsoft.EntityFrameworkCore;

namespace HandballCompetitionManager.Repositories;

public class PlayerRepository
{
    private readonly HandballDbContext _context;

    public PlayerRepository(HandballDbContext context)
    {
        _context = context;
    }

    public List<Player> GetAll()
    {
        return BuildPlayerQuery().ToList();
    }

    public List<Player> Search(string? query)
    {
        var players = BuildPlayerQuery().ToList();

        if (string.IsNullOrWhiteSpace(query))
        {
            return players;
        }

        var trimmedQuery = query.Trim();
        return players.Where(p =>
            p.FullName.Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ||
            p.Position.ToString().Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ||
            p.JerseyNumber.ToString().Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ||
            (p.Team?.Name.Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ?? false))
            .ToList();
    }

    public List<Player> SearchForUser(string? query, int userId)
    {
        var players = BuildPlayerQuery()
            .Where(p =>
                p.Team != null &&
                p.Team.Competitions.Any(c =>
                    c.DeletedAt == null &&
                    c.Administrators.Any(a => a.Id == userId)))
            .ToList();

        if (string.IsNullOrWhiteSpace(query))
        {
            return players;
        }

        var trimmedQuery = query.Trim();
        return players.Where(p =>
            p.FullName.Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ||
            p.Position.ToString().Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ||
            p.JerseyNumber.ToString().Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ||
            (p.Team?.Name.Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ?? false))
            .ToList();
    }

    public List<Player> SearchForCoach(string? query, string coachName)
    {
        var players = BuildPlayerQuery()
            .Where(p => p.Team != null && p.Team.CoachName == coachName)
            .ToList();

        if (string.IsNullOrWhiteSpace(query))
        {
            return players;
        }

        var trimmedQuery = query.Trim();
        return players.Where(p =>
            p.FullName.Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ||
            p.Position.ToString().Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ||
            p.JerseyNumber.ToString().Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ||
            (p.Team?.Name.Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ?? false))
            .ToList();
    }

    public Player? GetById(int id)
    {
        return BuildPlayerQuery().FirstOrDefault(p => p.Id == id);
    }

    public Player? GetByIdForUser(int id, int userId)
    {
        return BuildPlayerQuery()
            .FirstOrDefault(p =>
                p.Id == id &&
                p.Team != null &&
                p.Team.Competitions.Any(c =>
                    c.DeletedAt == null &&
                    c.Administrators.Any(a => a.Id == userId)));
    }

    public Player? GetByIdForCoach(int id, string coachName)
    {
        return BuildPlayerQuery()
            .FirstOrDefault(p => p.Id == id && p.Team != null && p.Team.CoachName == coachName);
    }

    public List<Player> GetByTeamId(int teamId)
    {
        return BuildPlayerQuery().Where(p => p.TeamId == teamId).ToList();
    }

    public List<Player> GetByTeamIdForUser(int teamId, int userId)
    {
        return BuildPlayerQuery()
            .Where(p =>
                p.TeamId == teamId &&
                p.Team != null &&
                p.Team.Competitions.Any(c =>
                    c.DeletedAt == null &&
                    c.Administrators.Any(a => a.Id == userId)))
            .ToList();
    }

    public List<Player> GetByTeamIdForCoach(int teamId, string coachName)
    {
        return BuildPlayerQuery()
            .Where(p => p.TeamId == teamId && p.Team != null && p.Team.CoachName == coachName)
            .ToList();
    }

    public List<Player> GetByPosition(PlayerPosition position)
    {
        return _context.Players.Where(p => p.DeletedAt == null && p.Position == position).ToList();
    }

    public void Add(Player player)
    {
        _context.Players.Add(player);
        _context.SaveChanges();
    }

    public void Update(Player player)
    {
        _context.Players.Update(player);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var player = GetById(id);
        if (player != null)
        {
            player.DeletedAt = DateTime.UtcNow;
            _context.SaveChanges();
        }
    }

    private IQueryable<Player> BuildPlayerQuery()
    {
        return _context.Players
            .Where(p => p.DeletedAt == null)
            .Include(p => p.Team)
                .ThenInclude(t => t!.Competitions.Where(c => c.DeletedAt == null))
                    .ThenInclude(c => c.Administrators);
    }
}
