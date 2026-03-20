namespace HappyWedding.Api.Models.Dtos.Auth;

public record RefreshTokenRequestDto
{
    public Guid UserId { get; set; }
    public required string RefreshToken { get; set; } = string.Empty;
}