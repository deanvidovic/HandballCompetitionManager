using HandballCompetitionManager.Data;
using HandballCompetitionManager.Dtos;
using HandballCompetitionManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandballCompetitionManager.Controllers;

[ApiController]
[Route("api/groups")]
public class GroupPhasesApiController : ControllerBase
{
    private readonly HandballDbContext _context;

    public GroupPhasesApiController(HandballDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GroupPhaseDto>>> GetAll([FromQuery] string? query, [FromQuery] int? competitionId)
    {
        var groups = _context.GroupPhases
            .Where(group => group.Competition != null && group.Competition.DeletedAt == null)
            .Include(group => group.Competition)
            .Include(group => group.Teams.Where(team => team.DeletedAt == null))
            .Include(group => group.Matches)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var term = $"%{query.Trim()}%";
            groups = groups.Where(group =>
                EF.Functions.Like(group.Name, term) ||
                (group.Competition != null && EF.Functions.Like(group.Competition.Name, term)));
        }

        if (competitionId.HasValue)
        {
            groups = groups.Where(group => group.CompetitionId == competitionId.Value);
        }

        var result = await groups.OrderBy(group => group.CompetitionId).ThenBy(group => group.Name).ToListAsync();
        return result.Select(group => group.ToDto()).ToList();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GroupPhaseDto>> GetById(int id)
    {
        var group = await _context.GroupPhases
            .Where(group => group.Competition != null && group.Competition.DeletedAt == null)
            .Include(group => group.Competition)
            .Include(group => group.Teams.Where(team => team.DeletedAt == null))
            .Include(group => group.Matches)
            .FirstOrDefaultAsync(group => group.Id == id);

        return group == null ? NotFound() : group.ToDto();
    }

    [HttpPost]
    public async Task<ActionResult<GroupPhaseDto>> Create(GroupPhaseWriteDto dto)
    {
        if (!await CompetitionExists(dto.CompetitionId))
        {
            ModelState.AddModelError(nameof(dto.CompetitionId), "Competition was not found.");
            return ValidationProblem(ModelState);
        }

        var group = new GroupPhase
        {
            Name = dto.Name.Trim(),
            CompetitionId = dto.CompetitionId
        };

        await SetGroupTeams(group, dto.TeamIds);
        _context.GroupPhases.Add(group);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = group.Id }, group.ToDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, GroupPhaseWriteDto dto)
    {
        var group = await _context.GroupPhases
            .Where(group => group.Competition != null && group.Competition.DeletedAt == null)
            .Include(group => group.Teams)
            .FirstOrDefaultAsync(group => group.Id == id);

        if (group == null)
        {
            return NotFound();
        }

        if (!await CompetitionExists(dto.CompetitionId))
        {
            ModelState.AddModelError(nameof(dto.CompetitionId), "Competition was not found.");
            return ValidationProblem(ModelState);
        }

        group.Name = dto.Name.Trim();
        group.CompetitionId = dto.CompetitionId;
        await SetGroupTeams(group, dto.TeamIds);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var group = await _context.GroupPhases
            .Where(group => group.Competition != null && group.Competition.DeletedAt == null)
            .FirstOrDefaultAsync(group => group.Id == id);

        if (group == null)
        {
            return NotFound();
        }

        _context.GroupPhases.Remove(group);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private async Task<bool> CompetitionExists(int competitionId) =>
        await _context.Competitions.AnyAsync(competition => competition.Id == competitionId && competition.DeletedAt == null);

    private async Task SetGroupTeams(GroupPhase group, IEnumerable<int> teamIds)
    {
        group.Teams.Clear();
        var ids = teamIds.Distinct().ToList();
        var teams = await _context.Teams
            .Where(team => team.DeletedAt == null && ids.Contains(team.Id))
            .ToListAsync();

        foreach (var team in teams)
        {
            group.Teams.Add(team);
        }
    }
}
