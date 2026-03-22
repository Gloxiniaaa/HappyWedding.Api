using System.ComponentModel.DataAnnotations;

namespace HappyWedding.Api.DTOs.Wedding;

public record CreateWeddingDto
{
    [Required, MaxLength(100)]
    public required string Name1 { get; set; }

    [Required, MaxLength(100)]
    public required string Name2 { get; set; }

    [Required]
    public required DateTime Date { get; set; }

    [Required, MaxLength(300)]
    public required string Location { get; set; }

    [MaxLength(500)]
    public string Tagline { get; set; } = string.Empty;
}