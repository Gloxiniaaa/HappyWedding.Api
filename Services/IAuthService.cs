using HappyWedding.Api.Models.Domain;
using HappyWedding.Api.Models.Dtos.Auth;

namespace HappyWedding.Api.Services;

public interface IAuthService
{
    Task<User?> RegisterAsync(UserDto request);
    Task<TokenResponseDto?> LoginAsync(UserDto request);
    Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);
    Task<TokenResponseDto> LoginWithGoogleAsync(string email, string googleId);
}