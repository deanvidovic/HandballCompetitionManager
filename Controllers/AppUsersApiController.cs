using HandballCompetitionManager.Data;
using HandballCompetitionManager.Dtos;
using HandballCompetitionManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandballCompetitionManager.Controllers;

[ApiController]
[Route("api/users")]
public class AppUsersApiController : ControllerBase
{
    private readonly HandballDbContext _context;

    public AppUsersApiController(HandballDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUserDto>>> GetAll([FromQuery] string? query, [FromQuery] UserRole? role)
    {
        var users = _context.AppUsers
            .Where(user => user.DeletedAt == null)
            .Include(user => user.ManagedCompetitions.Where(competition => competition.DeletedAt == null))
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var term = $"%{query.Trim()}%";
            users = users.Where(user =>
                EF.Functions.Like(user.UserName, term) ||
                EF.Functions.Like(user.DisplayName, term) ||
                EF.Functions.Like(user.Email, term));
        }

        if (role.HasValue)
        {
            users = users.Where(user => user.Role == role.Value);
        }

        var result = await users.OrderBy(user => user.DisplayName).ToListAsync();
        return result.Select(user => user.ToDto()).ToList();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppUserDto>> GetById(int id)
    {
        var user = await _context.AppUsers
            .Where(user => user.DeletedAt == null)
            .Include(user => user.ManagedCompetitions.Where(competition => competition.DeletedAt == null))
            .FirstOrDefaultAsync(user => user.Id == id);

        return user == null ? NotFound() : user.ToDto();
    }

    [HttpPost]
    public async Task<ActionResult<AppUserDto>> Create(AppUserWriteDto dto)
    {
        var user = new AppUser
        {
            UserName = dto.Username.Trim(),
            DisplayName = dto.DisplayName.Trim(),
            Email = dto.Email.Trim(),
            EmailConfirmed = true,
            OIB = dto.OIB.Trim(),
            JMBG = dto.JMBG.Trim(),
            Role = dto.Role,
            DateOfBirth = dto.DateOfBirth?.Date,
            CreatedAt = DateTime.UtcNow
        };

        await SetManagedCompetitions(user, dto.ManagedCompetitionIds);
        _context.AppUsers.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user.ToDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, AppUserWriteDto dto)
    {
        var user = await _context.AppUsers
            .Where(user => user.DeletedAt == null)
            .Include(user => user.ManagedCompetitions)
            .FirstOrDefaultAsync(user => user.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        user.UserName = dto.Username.Trim();
        user.DisplayName = dto.DisplayName.Trim();
        user.Email = dto.Email.Trim();
        user.OIB = dto.OIB.Trim();
        user.JMBG = dto.JMBG.Trim();
        user.Role = dto.Role;
        user.DateOfBirth = dto.DateOfBirth?.Date;
        await SetManagedCompetitions(user, dto.ManagedCompetitionIds);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _context.AppUsers.FirstOrDefaultAsync(user => user.Id == id && user.DeletedAt == null);

        if (user == null)
        {
            return NotFound();
        }

        user.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private async Task SetManagedCompetitions(AppUser user, IEnumerable<int> competitionIds)
    {
        user.ManagedCompetitions.Clear();
        var ids = competitionIds.Distinct().ToList();
        var competitions = await _context.Competitions
            .Where(competition => competition.DeletedAt == null && ids.Contains(competition.Id))
            .ToListAsync();

        foreach (var competition in competitions)
        {
            user.ManagedCompetitions.Add(competition);
        }
    }
}
