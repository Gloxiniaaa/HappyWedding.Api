using HappyWedding.Api.Models.Domain;
using HappyWedding.Api.Models.Dtos.Auth;
using HappyWedding.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HappyWedding.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserDto request)
    {
        var user = await authService.RegisterAsync(request);
        if (user is null)
        {
            return BadRequest("User already exists");
        }

        return Ok(user);
    }


    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
    {
        var token = await authService.LoginAsync(request);
        if (token is null)
        {
            return BadRequest("Invalid username or password");
        }

        return Ok(token);
    }


    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokenResponseDto>> RefreshTokens(RefreshTokenRequestDto request)
    {
        var tokenResponse = await authService.RefreshTokensAsync(request);
        if (tokenResponse is null || tokenResponse.AccessToken is null || tokenResponse.RefreshToken is null)
        {
            return Unauthorized("Invalid refresh token");
        }

        return Ok(tokenResponse);
    }


    [Authorize]
    [HttpGet]
    public IActionResult AuthenticatedEndpoint()
    {
        return Ok("You are authenticated!");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public IActionResult AdminOnlyEndpoint()
    {
        return Ok("You are an admin!");
    }

}