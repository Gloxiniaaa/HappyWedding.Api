using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using HappyWedding.Api.Data;
using HappyWedding.Api.Models.Domain;
using HappyWedding.Api.Models.Dtos.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace HappyWedding.Api.Services;

public class AuthService(HappyWeddingDbContext context, IConfiguration configuration) : IAuthService
{
    public async Task<User?> RegisterAsync(UserDto request)
    {
        if (await context.Users.AnyAsync(u => u.Username == request.Username))          
        {
            return null; // User already exists
        }

        var user = new User
        {
            Username = request.Username,
        };
        var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);
        user.PasswordHash = hashedPassword;


        context.Users.Add(user);
        await context.SaveChangesAsync();
        await WeddingSeeder.SeedDefaultWeddingDataAsync(user, context);

        return user;
    }

    public async Task<TokenResponseDto?> LoginAsync(UserDto request)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (user is null)
        {
            return null;
        }


        var passwordVerificationResult = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            return null;
        }

        TokenResponseDto response = await CreateTokenResponseDto(user);
        return response;
    }

    private async Task<TokenResponseDto> CreateTokenResponseDto(User user)
    {
        return new TokenResponseDto()
        {
            AccessToken = GenerateJwtToken(user),
            RefreshToken = await GenerateAndSaveRefreshToken(user)
        };
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Authentication:Jwt:Key")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var tokenDesciptor = new JwtSecurityToken
        (
            issuer: configuration.GetValue<string>("Authentication:Jwt:Issuer"),
            audience: configuration.GetValue<string>("Authentication:Jwt:Audience"),
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds

        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDesciptor);
    }
    public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
    {
        var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
        if (user is null)
        {
            return null; // Invalid refresh token
        }
        return await CreateTokenResponseDto(user);
    }

    private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
    {
        var user = await context.Users.FindAsync(userId);
        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return null;
        }

        return user;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }


    private async Task<string> GenerateAndSaveRefreshToken(User user)
    {
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        context.Users.Update(user);
        await context.SaveChangesAsync();
        return refreshToken;
    }


    public async Task<TokenResponseDto> LoginWithGoogleAsync(string email, string googleId)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user is null)
        {
            user = new User { Username = email, Email = email, GoogleId = googleId};

            context.Users.Add(user);
            await context.SaveChangesAsync();
            await WeddingSeeder.SeedDefaultWeddingDataAsync(user, context);
        }
        return await CreateTokenResponseDto(user);
    }
}