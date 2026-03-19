// CreateGuestDto.cs
using HappyWedding.Api.Models.Domain;

namespace HappyWedding.Api.Models.Dtos.Guest;
public class CreateGuestDto
{
    public string Name { get; set; } = string.Empty;
    public string? Note { get; set; }
    public int SeatCount { get; set; } = 1;
    public Side Side { get; set; }
}
