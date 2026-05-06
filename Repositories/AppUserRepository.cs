using HandballCompetitionManager.Models;
using HandballCompetitionManager.Data;
using Microsoft.EntityFrameworkCore;

namespace HandballCompetitionManager.Repositories;

public class AppUserRepository
{
    private readonly HandballDbContext _context;

    public AppUserRepository(HandballDbContext context)
    {
        _context = context;
    }

    public List<AppUser> GetAll()
    {
        return _context.AppUsers.Include(u => u.ManagedCompetitions).ToList();
    }

    public AppUser? GetById(int id)
    {
        return _context.AppUsers.Include(u => u.ManagedCompetitions).FirstOrDefault(u => u.Id == id);
    }

    public List<AppUser> GetByRole(UserRole role)
    {
        return _context.AppUsers.Where(u => u.Role == role).ToList();
    }

    public void Add(AppUser user)
    {
        _context.AppUsers.Add(user);
        _context.SaveChanges();
    }

    public void Update(AppUser user)
    {
        _context.AppUsers.Update(user);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var user = GetById(id);
        if (user != null)
        {
            _context.AppUsers.Remove(user);
            _context.SaveChanges();
        }
    }
}
