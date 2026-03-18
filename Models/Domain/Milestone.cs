namespace HappyWedding.Api.Models.Domain;

// Assuming this is your Milestone from previous conversation
public class Milestone
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title    { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public DateTime Date   { get; set; }
    public bool Completed  { get; set; }
    public string Emoji    { get; set; } = string.Empty;

    // Foreign key property (the "many" side)
    public Guid WeddingId { get; set; }           // ← required for EF Core

    // Navigation property back to Wedding (optional but very useful)
    public Wedding? Wedding { get; set; }         // ← nullable because EF can be lazy
}