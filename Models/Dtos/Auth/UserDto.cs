namespace HappyWedding.Api.Models.Dtos.Auth;

public record UserDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
