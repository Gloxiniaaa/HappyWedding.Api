namespace HappyWedding.Api.DTOs.Wedding;

public record UpdateWeddingDto
{
    public required string Name1 { get; set; }

    public required string Name2 { get; set; }

    public required DateTime Date { get; set; }

    public required string Location { get; set; }

    public string Tagline { get; set; } = string.Empty;
}
