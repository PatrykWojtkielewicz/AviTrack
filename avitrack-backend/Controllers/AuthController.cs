using Microsoft.AspNetCore.Mvc;
using AviTrack.Api.DTOs;
using AviTrack.Api.Services;

namespace AviTrack.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await _authService.Register(request);

        if (result is null)
        {
            return Conflict("Email is already in use");
        }

        Response.Cookies.Append(
            "token",
            result.Token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            }
        );

        return Ok(new { username = result.Username });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _authService.Login(request);

        if (result is null)
        {
            return Conflict("Invalid credentials");
        }

        Response.Cookies.Append(
            "token",
            result.Token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            }
        );

        return Ok(new { username = result.Username });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("token");
        return Ok();
    }
}