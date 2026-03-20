
using HappyWedding.Api.Models.Dtos.Milestone;

namespace HappyWedding.Api.Models.Dtos.Invitation;

public record WeddingForGuestDto
{
    public Guid Id { get; set; }
    public string Name1 { get; set; } = string.Empty;
    public string Name2 { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Tagline { get; set; } = string.Empty;
    public List<MilestoneResponseDto> Milestones { get; set; } = new();
}