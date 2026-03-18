namespace HappyWedding.Api.Models.Domain;

public class Wedding
{
    public Guid Id { get; set; } = Guid.NewGuid();   // ← almost always needed
    public string UserId { get; set; } = null!;
    
    public string Name1 { get; set; } = string.Empty;
    public string Name2 { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Tagline { get; set; } = string.Empty;

    // Navigation property (the "one" side)
    public List<Milestone> Milestones { get; set; } = new();   // ← modern & clean
    // or: public ICollection<Milestone> Milestones { get; set; } = new List<Milestone>();
}