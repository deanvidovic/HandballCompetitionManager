using HandballCompetitionManager.Data;
using HandballCompetitionManager.Dtos;
using HandballCompetitionManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandballCompetitionManager.Controllers;

[ApiController]
[Route("api/teams")]
public class TeamsApiController : ControllerBase
{
    private readonly HandballDbContext _context;

    public TeamsApiController(HandballDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamDto>>> GetAll([FromQuery] string? query, [FromQuery] string? city, [FromQuery] int? foundedAfter)
    {
        var teams = _context.Teams
            .Where(team => team.DeletedAt == null)
            .Include(team => team.Players.Where(player => player.DeletedAt == null))
            .Include(team => team.Competitions.Where(competition => competition.DeletedAt == null))
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var term = $"%{query.Trim()}%";
            teams = teams.Where(team =>
                EF.Functions.Like(team.Name, term) ||
                EF.Functions.Like(team.CoachName, term) ||
                EF.Functions.Like(team.HomeCity, term) ||
                EF.Functions.Like(team.HomeArena, term));
        }

        if (!string.IsNullOrWhiteSpace(city))
        {
            teams = teams.Where(team => team.HomeCity == city.Trim());
        }

        if (foundedAfter.HasValue)
        {
            teams = teams.Where(team => team.FoundedYear >= foundedAfter.Value);
        }

        var result = await teams.OrderBy(team => team.Name).ToListAsync();
        return result.Select(team => team.ToDto()).ToList();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TeamDto>> GetById(int id)
    {
        var team = await _context.Teams
            .Where(team => team.DeletedAt == null)
            .Include(team => team.Players.Where(player => player.DeletedAt == null))
            .Include(team => team.Competitions.Where(competition => competition.DeletedAt == null))
            .FirstOrDefaultAsync(team => team.Id == id);

        return team == null ? NotFound() : team.ToDto();
    }

    [HttpPost]
    public async Task<ActionResult<TeamDto>> Create(TeamWriteDto dto)
    {
        var team = new Team
        {
            Name = dto.Name.Trim(),
            CoachName = dto.CoachName.Trim(),
            HomeCity = dto.HomeCity.Trim(),
            FoundedYear = dto.FoundedYear,
            HomeArena = dto.HomeArena.Trim()
        };

        await SetTeamRelations(team, dto.CompetitionIds, dto.GroupPhaseIds);
        _context.Teams.Add(team);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = team.Id }, team.ToDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, TeamWriteDto dto)
    {
        var team = await _context.Teams
            .Where(team => team.DeletedAt == null)
            .Include(team => team.Competitions)
            .Include(team => team.GroupPhases)
            .FirstOrDefaultAsync(team => team.Id == id);

        if (team == null)
        {
            return NotFound();
        }

        team.Name = dto.Name.Trim();
        team.CoachName = dto.CoachName.Trim();
        team.HomeCity = dto.HomeCity.Trim();
        team.FoundedYear = dto.FoundedYear;
        team.HomeArena = dto.HomeArena.Trim();
        await SetTeamRelations(team, dto.CompetitionIds, dto.GroupPhaseIds);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var team = await _context.Teams
            .Where(team => team.DeletedAt == null)
            .Include(team => team.Players.Where(player => player.DeletedAt == null))
            .FirstOrDefaultAsync(team => team.Id == id);

        if (team == null)
        {
            return NotFound();
        }

        var deletedAt = DateTime.UtcNow;
        team.DeletedAt = deletedAt;

        foreach (var player in team.Players)
        {
            player.DeletedAt = deletedAt;
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    private async Task SetTeamRelations(Team team, IEnumerable<int> competitionIds, IEnumerable<int> groupPhaseIds)
    {
        team.Competitions.Clear();
        team.GroupPhases.Clear();

        var competitionIdList = competitionIds.Distinct().ToList();
        var groupPhaseIdList = groupPhaseIds.Distinct().ToList();
        var competitions = await _context.Competitions
            .Where(competition => competition.DeletedAt == null && competitionIdList.Contains(competition.Id))
            .ToListAsync();
        var groups = await _context.GroupPhases
            .Where(group => group.Competition != null && group.Competition.DeletedAt == null && groupPhaseIdList.Contains(group.Id))
            .ToListAsync();

        foreach (var competition in competitions)
        {
            team.Competitions.Add(competition);
        }

        foreach (var group in groups)
        {
            team.GroupPhases.Add(group);
        }
    }
}
