using System.ComponentModel.DataAnnotations;

namespace HappyWedding.Api.DTOs.Milestone;

public class UpdateMilestoneDto
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Subtitle { get; set; } = string.Empty;

    [Required]
    public DateTime Date { get; set; }

    public bool Completed { get; set; }

    [MaxLength(10)]
    public string Emoji { get; set; } = string.Empty;
}