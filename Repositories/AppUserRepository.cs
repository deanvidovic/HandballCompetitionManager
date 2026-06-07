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
        return _context.AppUsers
            .Where(u => u.DeletedAt == null)
            .Include(u => u.ManagedCompetitions.Where(c => c.DeletedAt == null))
            .ToList();
    }

    public List<AppUser> Search(string? query)
    {
        var users = _context.AppUsers
            .Where(u => u.DeletedAt == null)
            .Include(u => u.ManagedCompetitions.Where(c => c.DeletedAt == null))
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var term = $"%{query.Trim()}%";
            users = users.Where(u =>
                EF.Functions.Like(u.DisplayName, term) ||
                EF.Functions.Like(u.Username, term) ||
                EF.Functions.Like(u.Email, term));
        }

        return users.ToList()
            .Where(u => string.IsNullOrWhiteSpace(query) ||
                u.DisplayName.Contains(query.Trim(), StringComparison.OrdinalIgnoreCase) ||
                u.Username.Contains(query.Trim(), StringComparison.OrdinalIgnoreCase) ||
                u.Email.Contains(query.Trim(), StringComparison.OrdinalIgnoreCase) ||
                u.Role.ToString().Contains(query.Trim(), StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public AppUser? GetById(int id)
    {
        return _context.AppUsers
            .Where(u => u.DeletedAt == null)
            .Include(u => u.ManagedCompetitions.Where(c => c.DeletedAt == null))
            .FirstOrDefault(u => u.Id == id);
    }

    public List<AppUser> GetByRole(UserRole role)
    {
        return _context.AppUsers.Where(u => u.DeletedAt == null && u.Role == role).ToList();
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
            user.DeletedAt = DateTime.UtcNow;
            _context.SaveChanges();
        }
    }
}
