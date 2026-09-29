using Booking.Api.Extensions;
using Booking.Application.DTOs.Auth;
using Booking.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace Booking_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

	[HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request,CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(request,cancellationToken);

        return result.IsSuccess ?  Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(request, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RefreshTokenAsync(request, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var rolesClaim = User.FindFirst("roles")?.Value;
        var roles = rolesClaim is null
            ? Enumerable.Empty<string>()
            : JsonSerializer.Deserialize<List<string>>(rolesClaim) ?? [];

        return Ok(new
        {
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            Email = User.FindFirstValue(ClaimTypes.Email),
            Roles = roles
        });
    }
}