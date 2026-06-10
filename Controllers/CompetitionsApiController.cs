using HandballCompetitionManager.Data;
using HandballCompetitionManager.Dtos;
using HandballCompetitionManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandballCompetitionManager.Controllers;

[ApiController]
[Route("api/competitions")]
public class CompetitionsApiController : ControllerBase
{
    private readonly HandballDbContext _context;

    public CompetitionsApiController(HandballDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CompetitionDto>>> GetAll([FromQuery] string? query, [FromQuery] string? city, [FromQuery] string? season)
    {
        var competitions = IncludeCompetitionGraph(_context.Competitions.Where(competition => competition.DeletedAt == null));

        if (!string.IsNullOrWhiteSpace(query))
        {
            var term = $"%{query.Trim()}%";
            competitions = competitions.Where(competition =>
                EF.Functions.Like(competition.Name, term) ||
                EF.Functions.Like(competition.City, term) ||
                EF.Functions.Like(competition.Season, term));
        }

        if (!string.IsNullOrWhiteSpace(city))
        {
            competitions = competitions.Where(competition => competition.City == city.Trim());
        }

        if (!string.IsNullOrWhiteSpace(season))
        {
            competitions = competitions.Where(competition => competition.Season == season.Trim());
        }

        var result = await competitions.OrderBy(competition => competition.StartDate).ToListAsync();
        return result.Select(competition => competition.ToDto()).ToList();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CompetitionDto>> GetById(int id)
    {
        var competition = await IncludeCompetitionGraph(_context.Competitions.Where(competition => competition.DeletedAt == null))
            .FirstOrDefaultAsync(competition => competition.Id == id);

        return competition == null ? NotFound() : competition.ToDto();
    }

    [HttpPost]
    public async Task<ActionResult<CompetitionDto>> Create(CompetitionWriteDto dto)
    {
        if (dto.StartDate.Date > dto.EndDate.Date)
        {
            ModelState.AddModelError(nameof(dto.StartDate), "Start date cannot be after end date.");
            return ValidationProblem(ModelState);
        }

        var competition = new Competition
        {
            Name = dto.Name.Trim(),
            Season = dto.Season.Trim(),
            StartDate = dto.StartDate.Date,
            EndDate = dto.EndDate.Date,
            City = dto.City.Trim()
        };

        await SetCompetitionRelations(competition, dto.TeamIds, dto.AdministratorIds);
        _context.Competitions.Add(competition);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = competition.Id }, competition.ToDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CompetitionWriteDto dto)
    {
        if (dto.StartDate.Date > dto.EndDate.Date)
        {
            ModelState.AddModelError(nameof(dto.StartDate), "Start date cannot be after end date.");
            return ValidationProblem(ModelState);
        }

        var competition = await IncludeCompetitionGraph(_context.Competitions.Where(competition => competition.DeletedAt == null))
            .FirstOrDefaultAsync(competition => competition.Id == id);

        if (competition == null)
        {
            return NotFound();
        }

        competition.Name = dto.Name.Trim();
        competition.Season = dto.Season.Trim();
        competition.StartDate = dto.StartDate.Date;
        competition.EndDate = dto.EndDate.Date;
        competition.City = dto.City.Trim();
        await SetCompetitionRelations(competition, dto.TeamIds, dto.AdministratorIds);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var competition = await _context.Competitions.FirstOrDefaultAsync(competition => competition.Id == id && competition.DeletedAt == null);

        if (competition == null)
        {
            return NotFound();
        }

        competition.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private static IQueryable<Competition> IncludeCompetitionGraph(IQueryable<Competition> competitions) =>
        competitions
            .Include(competition => competition.Teams.Where(team => team.DeletedAt == null))
            .Include(competition => competition.Groups)
            .Include(competition => competition.Administrators.Where(user => user.DeletedAt == null));

    private async Task SetCompetitionRelations(Competition competition, IEnumerable<int> teamIds, IEnumerable<int> administratorIds)
    {
        competition.Teams.Clear();
        competition.Administrators.Clear();

        var teamIdList = teamIds.Distinct().ToList();
        var administratorIdList = administratorIds.Distinct().ToList();
        var teams = await _context.Teams
            .Where(team => team.DeletedAt == null && teamIdList.Contains(team.Id))
            .ToListAsync();
        var administrators = await _context.AppUsers
            .Where(user => user.DeletedAt == null && administratorIdList.Contains(user.Id))
            .ToListAsync();

        foreach (var team in teams)
        {
            competition.Teams.Add(team);
        }

        foreach (var administrator in administrators)
        {
            competition.Administrators.Add(administrator);
        }
    }
}
