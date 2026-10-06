using System.Security.Claims;

namespace Booking.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
	public static Guid? GetUserId(this ClaimsPrincipal user)
		=> Guid.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;
}