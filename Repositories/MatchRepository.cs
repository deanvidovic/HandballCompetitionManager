using HandballCompetitionManager.Models;
using HandballCompetitionManager.Data;
using Microsoft.EntityFrameworkCore;

namespace HandballCompetitionManager.Repositories;

public class MatchRepository
{
    private readonly HandballDbContext _context;

    public MatchRepository(HandballDbContext context)
    {
        _context = context;
    }

    public List<Match> GetAll()
    {
        return BuildMatchQuery().ToList();
    }

    public List<Match> Search(string? query)
    {
        var matches = BuildMatchQuery().ToList();

        if (string.IsNullOrWhiteSpace(query))
        {
            return matches;
        }

        var trimmedQuery = query.Trim();
        return matches.Where(m =>
            m.Status.ToString().Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ||
            m.MaintenanceHall.Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ||
            m.RoundNumber.ToString().Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ||
            (m.HomeTeam?.Name.Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (m.AwayTeam?.Name.Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (m.Competition?.Name.Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ?? false))
            .ToList();
    }

    public List<Match> SearchForUser(string? query, int userId)
    {
        var matches = BuildMatchQuery()
            .Where(m =>
                m.Competition != null &&
                m.Competition.Administrators.Any(a => a.Id == userId))
            .ToList();

        if (string.IsNullOrWhiteSpace(query))
        {
            return matches;
        }

        var trimmedQuery = query.Trim();
        return matches.Where(m =>
            m.Status.ToString().Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ||
            m.MaintenanceHall.Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ||
            m.RoundNumber.ToString().Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ||
            (m.HomeTeam?.Name.Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (m.AwayTeam?.Name.Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (m.Competition?.Name.Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ?? false))
            .ToList();
    }

    public List<Match> SearchForCoach(string? query, string coachName)
    {
        var matches = BuildMatchQuery()
            .Where(m =>
                (m.HomeTeam != null && m.HomeTeam.CoachName == coachName) ||
                (m.AwayTeam != null && m.AwayTeam.CoachName == coachName))
            .ToList();

        if (string.IsNullOrWhiteSpace(query))
        {
            return matches;
        }

        var trimmedQuery = query.Trim();
        return matches.Where(m =>
            m.Status.ToString().Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ||
            m.MaintenanceHall.Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ||
            m.RoundNumber.ToString().Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ||
            (m.HomeTeam?.Name.Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (m.AwayTeam?.Name.Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (m.Competition?.Name.Contains(trimmedQuery, StringComparison.OrdinalIgnoreCase) ?? false))
            .ToList();
    }

    public Match? GetById(int id)
    {
        return BuildMatchQuery().FirstOrDefault(m => m.Id == id);
    }

    public Match? GetByIdForUser(int id, int userId)
    {
        return BuildMatchQuery()
            .FirstOrDefault(m =>
                m.Id == id &&
                m.Competition != null &&
                m.Competition.Administrators.Any(a => a.Id == userId));
    }

    public Match? GetByIdForCoach(int id, string coachName)
    {
        return BuildMatchQuery()
            .FirstOrDefault(m =>
                m.Id == id &&
                ((m.HomeTeam != null && m.HomeTeam.CoachName == coachName) ||
                 (m.AwayTeam != null && m.AwayTeam.CoachName == coachName)));
    }

    public List<Match> GetByCompetitionId(int competitionId)
    {
        return _context.Matches
            .Where(m =>
                m.CompetitionId == competitionId &&
                m.Competition != null && m.Competition.DeletedAt == null &&
                m.HomeTeam != null && m.HomeTeam.DeletedAt == null &&
                m.AwayTeam != null && m.AwayTeam.DeletedAt == null)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .ToList();
    }

    public List<Match> GetByStatusForUser(MatchStatus status, int userId)
    {
        return BuildMatchQuery()
            .Where(m =>
                m.Status == status &&
                m.Competition != null &&
                m.Competition.Administrators.Any(a => a.Id == userId))
            .ToList();
    }

    public List<Match> GetByStatusForCoach(MatchStatus status, string coachName)
    {
        return BuildMatchQuery()
            .Where(m =>
                m.Status == status &&
                ((m.HomeTeam != null && m.HomeTeam.CoachName == coachName) ||
                 (m.AwayTeam != null && m.AwayTeam.CoachName == coachName)))
            .ToList();
    }

    public List<Match> GetByGroupId(int groupId)
    {
        return _context.Matches
            .Where(m =>
                m.GroupId == groupId &&
                m.Competition != null && m.Competition.DeletedAt == null &&
                m.HomeTeam != null && m.HomeTeam.DeletedAt == null &&
                m.AwayTeam != null && m.AwayTeam.DeletedAt == null)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .ToList();
    }

    public List<Match> GetByTeamId(int teamId)
    {
        return _context.Matches
            .Where(m =>
                (m.HomeTeamId == teamId || m.AwayTeamId == teamId) &&
                m.Competition != null && m.Competition.DeletedAt == null &&
                m.HomeTeam != null && m.HomeTeam.DeletedAt == null &&
                m.AwayTeam != null && m.AwayTeam.DeletedAt == null)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .ToList();
    }

    public List<Match> GetByStatus(MatchStatus status)
    {
        return _context.Matches
            .Where(m =>
                m.Status == status &&
                m.Competition != null && m.Competition.DeletedAt == null &&
                m.HomeTeam != null && m.HomeTeam.DeletedAt == null &&
                m.AwayTeam != null && m.AwayTeam.DeletedAt == null)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .ToList();
    }

    public void Add(Match match)
    {
        _context.Matches.Add(match);
        _context.SaveChanges();
    }

    public void Update(Match match)
    {
        _context.Matches.Update(match);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var match = GetById(id);
        if (match != null)
        {
            _context.Matches.Remove(match);
            _context.SaveChanges();
        }
    }

    private IQueryable<Match> BuildMatchQuery()
    {
        return _context.Matches
            .Where(m =>
                m.Competition != null && m.Competition.DeletedAt == null &&
                m.HomeTeam != null && m.HomeTeam.DeletedAt == null &&
                m.AwayTeam != null && m.AwayTeam.DeletedAt == null)
            .Include(m => m.Competition)
                .ThenInclude(c => c!.Administrators)
            .Include(m => m.GroupPhase)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam);
    }
}
