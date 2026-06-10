using HandballCompetitionManager.Models;

namespace HandballCompetitionManager.Dtos;

public record TeamSummaryDto(int Id, string Name, string CoachName, string HomeCity);

public record PlayerSummaryDto(int Id, string FullName, int JerseyNumber, PlayerPosition Position);

public record CompetitionSummaryDto(int Id, string Name, string Season, string City);

public record GroupPhaseSummaryDto(int Id, string Name, int CompetitionId);

public record AppUserSummaryDto(int Id, string Username, string DisplayName, string Email, UserRole Role);

public record MatchTeamDto(int Id, string Name);

public record MatchSummaryDto(int Id, int RoundNumber, DateTime Kickoff, MatchStatus Status);
