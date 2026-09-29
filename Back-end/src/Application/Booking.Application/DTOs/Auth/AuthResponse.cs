namespace Booking.Application.DTOs.Auth;

public class AuthResponse
{
    public string Message { get; set; } = string.Empty;
    public string? UserId { get; set; }
    public string? Email { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
}