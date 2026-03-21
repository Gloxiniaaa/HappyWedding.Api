namespace HappyWedding.Api.Models.Domain;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // Default role
    public string? RefreshToken { get; set; } = string.Empty;
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public string? GoogleId { get; set; }
    public string? Email { get; set; }
}