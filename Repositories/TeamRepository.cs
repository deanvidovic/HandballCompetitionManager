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
        return _context.Teams.Include(t => t.Players).Include(t => t.Competitions).ToList();
    }

    public Team? GetById(int id)
    {
        return _context.Teams.Include(t => t.Players).Include(t => t.Competitions).FirstOrDefault(t => t.Id == id);
    }

    public List<Team> GetByCity(string city)
    {
        return _context.Teams.Where(t => t.HomeCity == city).ToList();
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
            _context.Teams.Remove(team);
            _context.SaveChanges();
        }
    }
}
