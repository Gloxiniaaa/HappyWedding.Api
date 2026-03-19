namespace HappyWedding.Api.Models.Domain;

public class Guest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Note { get; set; }
    public int SeatCount { get; set; } = 1;
    public bool Confirmed { get; set; } = false;
    public Side Side { get; set; }  // groom | bride

    public Guid WeddingId { get; set; }
    public Wedding Wedding { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

public enum Side { Groom, Bride }