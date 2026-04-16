namespace HandballCompetitionManager.Repositories.Mock;

using HandballCompetitionManager.Models;

public class MatchMockRepository
{
    private static readonly List<Match> _matches = new()
    {
        // Competition 1, Group A matches
        new Match
        {
            Id = 1,
            CompetitionId = 1,
            GroupId = 1,
            RoundNumber = 1,
            Kickoff = new DateTime(2026, 2, 1, 18, 0, 0),
            HomeTeamId = 1,
            AwayTeamId = 2,
            HomeScore = 28,
            AwayScore = 25,
            MaintenanceHall = "Pabellon Mladost",
            Status = MatchStatus.Finished
        },
        new Match
        {
            Id = 2,
            CompetitionId = 1,
            GroupId = 1,
            RoundNumber = 1,
            Kickoff = new DateTime(2026, 2, 1, 20, 0, 0),
            HomeTeamId = 3,
            AwayTeamId = 4,
            HomeScore = 31,
            AwayScore = 27,
            MaintenanceHall = "Poljud Arena",
            Status = MatchStatus.Finished
        },
        // Competition 1, Group B matches
        new Match
        {
            Id = 3,
            CompetitionId = 1,
            GroupId = 2,
            RoundNumber = 1,
            Kickoff = new DateTime(2026, 2, 2, 18, 0, 0),
            HomeTeamId = 5,
            AwayTeamId = 1,
            HomeScore = 22,
            AwayScore = 26,
            MaintenanceHall = "Zaro",
            Status = MatchStatus.Finished
        },
        new Match
        {
            Id = 4,
            CompetitionId = 1,
            GroupId = 2,
            RoundNumber = 2,
            Kickoff = new DateTime(2026, 2, 8, 19, 0, 0),
            HomeTeamId = 2,
            AwayTeamId = 3,
            HomeScore = 24,
            AwayScore = 29,
            MaintenanceHall = "Gradski vrt",
            Status = MatchStatus.Finished
        },
        // Scheduled matches (future)
        new Match
        {
            Id = 5,
            CompetitionId = 1,
            GroupId = 1,
            RoundNumber = 2,
            Kickoff = new DateTime(2026, 4, 15, 18, 0, 0),
            HomeTeamId = 1,
            AwayTeamId = 4,
            HomeScore = 0,
            AwayScore = 0,
            MaintenanceHall = "Pabellon Mladost",
            Status = MatchStatus.Scheduled
        },
        new Match
        {
            Id = 6,
            CompetitionId = 1,
            GroupId = 1,
            RoundNumber = 2,
            Kickoff = new DateTime(2026, 4, 15, 20, 0, 0),
            HomeTeamId = 3,
            AwayTeamId = 2,
            HomeScore = 0,
            AwayScore = 0,
            MaintenanceHall = "Poljud Arena",
            Status = MatchStatus.Scheduled
        },
        new Match
        {
            Id = 7,
            CompetitionId = 2,
            GroupId = 5,
            RoundNumber = 1,
            Kickoff = new DateTime(2026, 4, 20, 19, 0, 0),
            HomeTeamId = 1,
            AwayTeamId = 3,
            HomeScore = 0,
            AwayScore = 0,
            MaintenanceHall = "Pabellon Mladost",
            Status = MatchStatus.Scheduled
        },
        new Match
        {
            Id = 8,
            CompetitionId = 2,
            GroupId = 5,
            RoundNumber = 1,
            Kickoff = new DateTime(2026, 4, 20, 21, 0, 0),
            HomeTeamId = 2,
            AwayTeamId = 5,
            HomeScore = 0,
            AwayScore = 0,
            MaintenanceHall = "Gradski vrt",
            Status = MatchStatus.Scheduled
        },
        // Live match (example)
        new Match
        {
            Id = 9,
            CompetitionId = 1,
            GroupId = 2,
            RoundNumber = 3,
            Kickoff = new DateTime(2026, 4, 14, 18, 0, 0),
            HomeTeamId = 4,
            AwayTeamId = 5,
            HomeScore = 15,
            AwayScore = 14,
            MaintenanceHall = "Gradski vrt",
            Status = MatchStatus.Live
        }
    };

    public List<Match> GetAll()
    {
        return _matches;
    }

    public Match? GetById(int id)
    {
        return _matches.FirstOrDefault(m => m.Id == id);
    }

    public List<Match> GetByCompetitionId(int competitionId)
    {
        return _matches.Where(m => m.CompetitionId == competitionId).ToList();
    }

    public List<Match> GetByGroupId(int groupId)
    {
        return _matches.Where(m => m.GroupId == groupId).ToList();
    }

    public List<Match> GetByTeamId(int teamId)
    {
        return _matches.Where(m => m.HomeTeamId == teamId || m.AwayTeamId == teamId).ToList();
    }

    public List<Match> GetByStatus(MatchStatus status)
    {
        return _matches.Where(m => m.Status == status).ToList();
    }

    public List<Match> GetUpcoming()
    {
        var now = DateTime.Now;
        return _matches.Where(m => m.Kickoff > now && m.Status == MatchStatus.Scheduled)
            .OrderBy(m => m.Kickoff)
            .ToList();
    }

    public List<Match> GetLive()
    {
        return _matches.Where(m => m.Status == MatchStatus.Live).ToList();
    }
}
