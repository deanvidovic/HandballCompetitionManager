using HandballCompetitionManager.Data;
using HandballCompetitionManager.Dtos;
using HandballCompetitionManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandballCompetitionManager.Controllers;

[ApiController]
[Route("api/matches")]
public class MatchesApiController : ControllerBase
{
    private readonly HandballDbContext _context;

    public MatchesApiController(HandballDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MatchDto>>> GetAll([FromQuery] string? query, [FromQuery] int? competitionId, [FromQuery] int? teamId, [FromQuery] MatchStatus? status)
    {
        var matches = IncludeMatchGraph(_context.Matches.Where(match =>
            match.Competition != null && match.Competition.DeletedAt == null &&
            match.HomeTeam != null && match.HomeTeam.DeletedAt == null &&
            match.AwayTeam != null && match.AwayTeam.DeletedAt == null));

        if (competitionId.HasValue)
        {
            matches = matches.Where(match => match.CompetitionId == competitionId.Value);
        }

        if (teamId.HasValue)
        {
            matches = matches.Where(match => match.HomeTeamId == teamId.Value || match.AwayTeamId == teamId.Value);
        }

        if (status.HasValue)
        {
            matches = matches.Where(match => match.Status == status.Value);
        }

        var result = await matches.OrderBy(match => match.Kickoff).ToListAsync();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var trimmedQuery = query.Trim();
            result = result.Where(match =>
                match.Status.ToString().Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ||
                match.MaintenanceHall.Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ||
                match.RoundNumber.ToString().Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ||
                (match.HomeTeam?.Name.Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (match.AwayTeam?.Name.Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (match.Competition?.Name.Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ?? false))
                .ToList();
        }

        return result.Select(match => match.ToDto()).ToList();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MatchDto>> GetById(int id)
    {
        var match = await IncludeMatchGraph(_context.Matches.Where(match =>
                match.Competition != null && match.Competition.DeletedAt == null &&
                match.HomeTeam != null && match.HomeTeam.DeletedAt == null &&
                match.AwayTeam != null && match.AwayTeam.DeletedAt == null))
            .FirstOrDefaultAsync(match => match.Id == id);

        return match == null ? NotFound() : match.ToDto();
    }

    [HttpPost]
    public async Task<ActionResult<MatchDto>> Create(MatchWriteDto dto)
    {
        if (!await ValidateMatchReferences(dto))
        {
            return ValidationProblem(ModelState);
        }

        var match = new Match
        {
            CompetitionId = dto.CompetitionId,
            GroupId = dto.GroupId,
            RoundNumber = dto.RoundNumber,
            Kickoff = dto.Kickoff,
            HomeTeamId = dto.HomeTeamId,
            AwayTeamId = dto.AwayTeamId,
            HomeScore = dto.HomeScore,
            AwayScore = dto.AwayScore,
            MaintenanceHall = dto.MaintenanceHall.Trim(),
            Status = dto.Status
        };

        _context.Matches.Add(match);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = match.Id }, (await IncludeMatchGraph(_context.Matches).FirstAsync(m => m.Id == match.Id)).ToDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, MatchWriteDto dto)
    {
        var match = await _context.Matches.FirstOrDefaultAsync(match => match.Id == id);

        if (match == null)
        {
            return NotFound();
        }

        if (!await ValidateMatchReferences(dto))
        {
            return ValidationProblem(ModelState);
        }

        match.CompetitionId = dto.CompetitionId;
        match.GroupId = dto.GroupId;
        match.RoundNumber = dto.RoundNumber;
        match.Kickoff = dto.Kickoff;
        match.HomeTeamId = dto.HomeTeamId;
        match.AwayTeamId = dto.AwayTeamId;
        match.HomeScore = dto.HomeScore;
        match.AwayScore = dto.AwayScore;
        match.MaintenanceHall = dto.MaintenanceHall.Trim();
        match.Status = dto.Status;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var match = await _context.Matches.FirstOrDefaultAsync(match => match.Id == id);

        if (match == null)
        {
            return NotFound();
        }

        _context.Matches.Remove(match);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private static IQueryable<Match> IncludeMatchGraph(IQueryable<Match> matches) =>
        matches
            .Include(match => match.Competition)
            .Include(match => match.GroupPhase)
            .Include(match => match.HomeTeam)
            .Include(match => match.AwayTeam);

    private async Task<bool> ValidateMatchReferences(MatchWriteDto dto)
    {
        if (dto.HomeTeamId == dto.AwayTeamId)
        {
            ModelState.AddModelError(nameof(dto.AwayTeamId), "Home and away team must be different.");
        }

        var competitionExists = await _context.Competitions.AnyAsync(competition => competition.Id == dto.CompetitionId && competition.DeletedAt == null);
        var groupExists = await _context.GroupPhases.AnyAsync(group =>
            group.Id == dto.GroupId &&
            group.CompetitionId == dto.CompetitionId &&
            group.Competition != null &&
            group.Competition.DeletedAt == null);
        var homeTeamExists = await _context.Teams.AnyAsync(team => team.Id == dto.HomeTeamId && team.DeletedAt == null);
        var awayTeamExists = await _context.Teams.AnyAsync(team => team.Id == dto.AwayTeamId && team.DeletedAt == null);

        if (!competitionExists)
        {
            ModelState.AddModelError(nameof(dto.CompetitionId), "Competition was not found.");
        }

        if (!groupExists)
        {
            ModelState.AddModelError(nameof(dto.GroupId), "Group was not found in selected competition.");
        }

        if (!homeTeamExists)
        {
            ModelState.AddModelError(nameof(dto.HomeTeamId), "Home team was not found.");
        }

        if (!awayTeamExists)
        {
            ModelState.AddModelError(nameof(dto.AwayTeamId), "Away team was not found.");
        }

        return ModelState.IsValid;
    }
}
