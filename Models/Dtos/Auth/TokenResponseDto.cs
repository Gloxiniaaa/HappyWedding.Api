namespace HappyWedding.Api.Models.Dtos.Auth;

public record TokenResponseDto
{
    public required string AccessToken { get; set; } = string.Empty;
    public required string RefreshToken { get; set; } = string.Empty;
}