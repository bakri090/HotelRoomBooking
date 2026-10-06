using Booking.Application.DTOs.Auth;

namespace Booking.Infrastructure.Auth;

public interface IJwtProvider
{
	(string token, int expiresIn) GenerateToken(GenerateTokenRequest user, IList<string> roles);
}