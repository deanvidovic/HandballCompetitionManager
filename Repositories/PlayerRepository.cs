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
        return _context.Players
            .Where(p => p.DeletedAt == null)
            .Include(p => p.Team)
            .ToList();
    }

    public List<Player> Search(string? query)
    {
        var players = _context.Players
            .Where(p => p.DeletedAt == null)
            .Include(p => p.Team)
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
        return _context.Players
            .Where(p => p.DeletedAt == null)
            .Include(p => p.Team)
            .FirstOrDefault(p => p.Id == id);
    }

    public List<Player> GetByTeamId(int teamId)
    {
        return _context.Players.Where(p => p.DeletedAt == null && p.TeamId == teamId).ToList();
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
}
