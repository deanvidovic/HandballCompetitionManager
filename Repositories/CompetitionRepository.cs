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
            .Include(c => c.Teams)
            .Include(c => c.Groups)
            .Include(c => c.Administrators)
            .ToList();
    }

    public Competition? GetById(int id)
    {
        return _context.Competitions
            .Include(c => c.Teams)
            .Include(c => c.Groups)
            .Include(c => c.Administrators)
            .FirstOrDefault(c => c.Id == id);
    }

    public List<Competition> GetByseason(string season)
    {
        return _context.Competitions.Where(c => c.Season == season).ToList();
    }

    public List<Competition> GetByCity(string city)
    {
        return _context.Competitions.Where(c => c.City == city).ToList();
    }

    public List<Competition> GetActive()
    {
        var now = DateTime.Now;
        return _context.Competitions.Where(c => c.StartDate <= now && c.EndDate >= now).ToList();
    }

    public void Add(Competition competition)
    {
        _context.Competitions.Add(competition);
        _context.SaveChanges();
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
            _context.Competitions.Remove(competition);
            _context.SaveChanges();
        }
    }
}
