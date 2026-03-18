using System.ComponentModel.DataAnnotations;

namespace HappyWedding.Api.DTOs.Wedding;

public class CreateWeddingDto
{
    [Required, MaxLength(100)]
    public string Name1 { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Name2 { get; set; } = string.Empty;

    [Required]
    public DateTime Date { get; set; }

    [Required, MaxLength(300)]
    public string Location { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Tagline { get; set; } = string.Empty;
}