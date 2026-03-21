using System.Security.Claims;
using HappyWedding.Api.Models.Domain;
using HappyWedding.Api.Models.Dtos.Auth;
using HappyWedding.Api.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
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


    [HttpGet("google-login")]
    public IActionResult GoogleLogin()
    {
        var redirectUrl = Url.Action("GoogleCallback", "Auth");
        var props = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(props, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google-callback")]
    public async Task<ActionResult<TokenResponseDto>> GoogleCallback()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (!result.Succeeded) return Unauthorized();

        var email = result.Principal.FindFirstValue(ClaimTypes.Email);
        if (email is null) return BadRequest("Email claim not found");
        var googleId = result.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (googleId is null) return BadRequest("Google ID claim not found");
        var token = await authService.LoginWithGoogleAsync(email, googleId);
        return Ok(token);
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