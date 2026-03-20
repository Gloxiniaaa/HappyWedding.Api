namespace HappyWedding.Api.Models.Dtos.Milestone;

public record UpdateMilestoneDto
{
    public required string Title { get; set; } = string.Empty;

    public string Subtitle { get; set; } = string.Empty;

    public required DateTime Date { get; set; }

    public bool Completed { get; set; }

    public string Emoji { get; set; } = string.Empty;
}