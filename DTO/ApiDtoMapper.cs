using HandballCompetitionManager.Models;

namespace HandballCompetitionManager.Dtos;

public static class ApiDtoMapper
{
    public static TeamSummaryDto ToSummaryDto(this Team team) =>
        new(team.Id, team.Name, team.CoachName, team.HomeCity);

    public static PlayerSummaryDto ToSummaryDto(this Player player) =>
        new(player.Id, player.FullName, player.JerseyNumber, player.Position);

    public static CompetitionSummaryDto ToSummaryDto(this Competition competition) =>
        new(competition.Id, competition.Name, competition.Season, competition.City);

    public static GroupPhaseSummaryDto ToSummaryDto(this GroupPhase group) =>
        new(group.Id, group.Name, group.CompetitionId);

    public static AppUserSummaryDto ToSummaryDto(this AppUser user) =>
        new(user.Id, user.UserName ?? string.Empty, user.DisplayName, user.Email, user.Role);

    public static MatchSummaryDto ToSummaryDto(this Match match) =>
        new(match.Id, match.RoundNumber, match.Kickoff, match.Status);

    public static AppUserDto ToDto(this AppUser user) =>
        new(
            user.Id,
            user.UserName ?? string.Empty,
            user.DisplayName,
            user.Email,
            user.Role,
            user.DateOfBirth,
            user.CreatedAt,
            user.ManagedCompetitions
                .Where(competition => competition.DeletedAt == null)
                .Select(competition => competition.ToSummaryDto())
                .ToList());

    public static CompetitionDto ToDto(this Competition competition) =>
        new(
            competition.Id,
            competition.Name,
            competition.Season,
            competition.StartDate,
            competition.EndDate,
            competition.City,
            competition.Teams
                .Where(team => team.DeletedAt == null)
                .Select(team => team.ToSummaryDto())
                .ToList(),
            competition.Groups
                .Select(group => group.ToSummaryDto())
                .ToList(),
            competition.Administrators
                .Where(user => user.DeletedAt == null)
                .Select(user => user.ToSummaryDto())
                .ToList());

    public static TeamDto ToDto(this Team team) =>
        new(
            team.Id,
            team.Name,
            team.CoachName,
            team.HomeCity,
            team.FoundedYear,
            team.HomeArena,
            team.Players
                .Where(player => player.DeletedAt == null)
                .Select(player => player.ToSummaryDto())
                .ToList(),
            team.Competitions
                .Where(competition => competition.DeletedAt == null)
                .Select(competition => competition.ToSummaryDto())
                .ToList());

    public static PlayerDto ToDto(this Player player) =>
        new(
            player.Id,
            player.FirstName,
            player.LastName,
            player.FullName,
            player.BirthDate,
            player.JerseyNumber,
            player.Position,
            player.GoalsScored,
            player.Assists,
            player.Team is { DeletedAt: null } ? player.Team.ToSummaryDto() : null);

    public static GroupPhaseDto ToDto(this GroupPhase group) =>
        new(
            group.Id,
            group.Name,
            group.Competition is { DeletedAt: null } ? group.Competition.ToSummaryDto() : null,
            group.Teams
                .Where(team => team.DeletedAt == null)
                .Select(team => team.ToSummaryDto())
                .ToList(),
            group.Matches
                .Select(match => match.ToSummaryDto())
                .ToList());

    public static MatchDto ToDto(this Match match) =>
        new(
            match.Id,
            match.Competition is { DeletedAt: null } ? match.Competition.ToSummaryDto() : null,
            match.GroupPhase?.ToSummaryDto(),
            match.RoundNumber,
            match.Kickoff,
            match.HomeTeam is { DeletedAt: null } ? new MatchTeamDto(match.HomeTeam.Id, match.HomeTeam.Name) : null,
            match.AwayTeam is { DeletedAt: null } ? new MatchTeamDto(match.AwayTeam.Id, match.AwayTeam.Name) : null,
            match.HomeScore,
            match.AwayScore,
            match.MaintenanceHall,
            match.Status,
            match.ReportFilePath,
            match.ReportFileName);
}
