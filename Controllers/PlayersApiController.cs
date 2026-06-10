using HandballCompetitionManager.Data;
using HandballCompetitionManager.Dtos;
using HandballCompetitionManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandballCompetitionManager.Controllers;

[ApiController]
[Route("api/players")]
public class PlayersApiController : ControllerBase
{
    private readonly HandballDbContext _context;

    public PlayersApiController(HandballDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlayerDto>>> GetAll([FromQuery] string? query, [FromQuery] int? teamId, [FromQuery] PlayerPosition? position)
    {
        var players = _context.Players
            .Where(player => player.DeletedAt == null && player.Team != null && player.Team.DeletedAt == null)
            .Include(player => player.Team)
            .AsQueryable();

        if (teamId.HasValue)
        {
            players = players.Where(player => player.TeamId == teamId.Value);
        }

        if (position.HasValue)
        {
            players = players.Where(player => player.Position == position.Value);
        }

        var result = await players.OrderBy(player => player.LastName).ThenBy(player => player.FirstName).ToListAsync();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var trimmedQuery = query.Trim();
            result = result.Where(player =>
                player.FullName.Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ||
                player.JerseyNumber.ToString().Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ||
                player.Position.ToString().Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ||
                (player.Team?.Name.Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ?? false))
                .ToList();
        }

        return result.Select(player => player.ToDto()).ToList();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PlayerDto>> GetById(int id)
    {
        var player = await _context.Players
            .Where(player => player.DeletedAt == null && player.Team != null && player.Team.DeletedAt == null)
            .Include(player => player.Team)
            .FirstOrDefaultAsync(player => player.Id == id);

        return player == null ? NotFound() : player.ToDto();
    }

    [HttpPost]
    public async Task<ActionResult<PlayerDto>> Create(PlayerWriteDto dto)
    {
        if (!await TeamExists(dto.TeamId))
        {
            ModelState.AddModelError(nameof(dto.TeamId), "Team was not found.");
            return ValidationProblem(ModelState);
        }

        var player = new Player
        {
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim(),
            BirthDate = dto.BirthDate.Date,
            JerseyNumber = dto.JerseyNumber,
            Position = dto.Position,
            TeamId = dto.TeamId,
            GoalsScored = dto.GoalsScored,
            Assists = dto.Assists
        };

        _context.Players.Add(player);
        await _context.SaveChangesAsync();
        await _context.Entry(player).Reference(p => p.Team).LoadAsync();

        return CreatedAtAction(nameof(GetById), new { id = player.Id }, player.ToDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, PlayerWriteDto dto)
    {
        var player = await _context.Players.FirstOrDefaultAsync(player => player.Id == id && player.DeletedAt == null);

        if (player == null)
        {
            return NotFound();
        }

        if (!await TeamExists(dto.TeamId))
        {
            ModelState.AddModelError(nameof(dto.TeamId), "Team was not found.");
            return ValidationProblem(ModelState);
        }

        player.FirstName = dto.FirstName.Trim();
        player.LastName = dto.LastName.Trim();
        player.BirthDate = dto.BirthDate.Date;
        player.JerseyNumber = dto.JerseyNumber;
        player.Position = dto.Position;
        player.TeamId = dto.TeamId;
        player.GoalsScored = dto.GoalsScored;
        player.Assists = dto.Assists;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var player = await _context.Players.FirstOrDefaultAsync(player => player.Id == id && player.DeletedAt == null);

        if (player == null)
        {
            return NotFound();
        }

        player.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private async Task<bool> TeamExists(int teamId) =>
        await _context.Teams.AnyAsync(team => team.Id == teamId && team.DeletedAt == null);
}
