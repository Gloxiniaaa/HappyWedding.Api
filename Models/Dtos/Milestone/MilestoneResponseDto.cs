namespace HappyWedding.Api.Models.Dtos.Milestone;

public class MilestoneResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public bool Completed { get; set; }
    public string Emoji { get; set; } = string.Empty;
    public Guid WeddingId { get; set; }
}