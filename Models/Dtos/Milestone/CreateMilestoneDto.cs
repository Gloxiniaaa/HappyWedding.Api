namespace HappyWedding.Api.Models.Dtos.Milestone;

public record CreateMilestoneDto
{
    public required string Title { get; set; } = string.Empty;

    public string Subtitle { get; set; } = string.Empty;

    public required DateTime Date { get; set; }

    public bool Completed { get; set; } = false;

    public string Emoji { get; set; } = string.Empty;
}