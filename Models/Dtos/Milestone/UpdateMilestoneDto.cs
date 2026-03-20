using System.ComponentModel.DataAnnotations;

namespace HappyWedding.Api.Models.Dtos.Milestone;

public record UpdateMilestoneDto
{
    [Required, MaxLength(200)]
    public required string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Subtitle { get; set; } = string.Empty;

    [Required]
    public required DateTime Date { get; set; }

    public bool Completed { get; set; }

    [MaxLength(10)]
    public string Emoji { get; set; } = string.Empty;
}