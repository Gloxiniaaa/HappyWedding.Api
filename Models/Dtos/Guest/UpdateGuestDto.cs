using HappyWedding.Api.Models.Domain;

namespace HappyWedding.Api.Models.Dtos.Guest;

// UpdateGuestDto.cs
public class UpdateGuestDto
{
    public string Name { get; set; } = string.Empty;
    public string? Note { get; set; }
    public int SeatCount { get; set; } = 1;
}
