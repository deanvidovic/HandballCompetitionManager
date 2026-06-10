using System.ComponentModel.DataAnnotations;
using HandballCompetitionManager.Models;

namespace HandballCompetitionManager.Dtos;

public record PlayerDto(
    int Id,
    string FirstName,
    string LastName,
    string FullName,
    DateTime BirthDate,
    int JerseyNumber,
    PlayerPosition Position,
    int GoalsScored,
    int Assists,
    TeamSummaryDto? Team);

public class PlayerWriteDto
{
    [Required, StringLength(80)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public DateTime BirthDate { get; set; }

    [Range(0, 99)]
    public int JerseyNumber { get; set; }

    public PlayerPosition Position { get; set; }

    [Range(1, int.MaxValue)]
    public int TeamId { get; set; }

    [Range(0, int.MaxValue)]
    public int GoalsScored { get; set; }

    [Range(0, int.MaxValue)]
    public int Assists { get; set; }
}
