using Booking.Application.DTOs.Auth;
using Booking.Application.Interfaces;
using Booking.Domain.Abstractions;
using Booking.Domain.Errors;
using Booking.Infrastructure.Auth;
using Booking.Infrastructure.Data;
using Booking.Infrastructure.Identity;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace Booking.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly ApplicationDbContext _dbContext;
    private readonly JwtOptions _jwtOptions;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtProvider jwtProvider,
        ApplicationDbContext dbContext,
        IOptions<JwtOptions> jwtOptions)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtProvider = jwtProvider;
        _dbContext = dbContext;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email.Substring(0, request.Email.IndexOf('@')),
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var error = result.Errors.First();

            return Result.Failure<AuthResponse>(
                    new Error(error.Code, error.Description, 400));
        }

        var roleResult = await _userManager.AddToRoleAsync(user, ApplicationRoles.Customer);

        if (!roleResult.Succeeded)
        {
            var error = roleResult.Errors.First();

            return Result.Failure<AuthResponse>(
                    new Error(error.Code, error.Description, 400));
        }

        var (token, refreshToken) = await IssueTokensAsync(user, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(BuildResponse("Registration successful.", user, token, refreshToken));
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);

        if (!result.Succeeded)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var (token, refreshToken) = await IssueTokensAsync(user, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(BuildResponse("Login successful.", user, token, refreshToken));
    }

    public async Task<Result<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var existing = await _dbContext.RefreshTokens
            .Include(rt => rt.CreatedBy)
            .SingleOrDefaultAsync(rt => rt.Token == request.RefreshToken, cancellationToken);

        if (existing is null || !existing.IsValid || existing.CreatedBy is null || !existing.CreatedBy.IsActive)
            return Result.Failure<AuthResponse>(UserErrors.InvalidToken);

        var (token, refreshToken) = await IssueTokensAsync(existing.CreatedBy, cancellationToken);

        existing.RevokedAt = DateTime.UtcNow;
        existing.ReplacedByToken = refreshToken;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(BuildResponse("Token refreshed successfully.", existing.CreatedBy, token, refreshToken));
    }

    private async Task<(string token, string refreshToken)> IssueTokensAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        var (token, _) = _jwtProvider.GenerateToken(user.Adapt<GenerateTokenRequest>(), userRoles);
        var refreshToken = GenerateRefreshToken();

        _dbContext.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            Expires = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpiryDays)
        });

        return (token, refreshToken);
    }

    private static AuthResponse BuildResponse(string message, ApplicationUser user, string token, string refreshToken)
    {
        return new AuthResponse
        {
            Message = message,
            UserId = user.Id.ToString(),
            Email = user.Email,
            Token = token,
            RefreshToken = refreshToken
        };
    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}