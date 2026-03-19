using HappyWedding.Api.Models.Domain;

namespace HappyWedding.Api.Models.Dtos.Guest;

public class GuestResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Note { get; set; }
    public int SeatCount { get; set; }
    public bool Confirmed { get; set; }
    public Side Side { get; set; }
    public Guid WeddingId { get; set; }
}