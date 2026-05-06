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
        return _context.Players.Include(p => p.Team).ToList();
    }

    public Player? GetById(int id)
    {
        return _context.Players.Include(p => p.Team).FirstOrDefault(p => p.Id == id);
    }

    public List<Player> GetByTeamId(int teamId)
    {
        return _context.Players.Where(p => p.TeamId == teamId).ToList();
    }

    public List<Player> GetByPosition(PlayerPosition position)
    {
        return _context.Players.Where(p => p.Position == position).ToList();
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
            _context.Players.Remove(player);
            _context.SaveChanges();
        }
    }
}
