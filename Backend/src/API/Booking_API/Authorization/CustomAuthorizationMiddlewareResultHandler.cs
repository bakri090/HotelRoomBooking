using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Booking.Api.Authorization;

public class CustomAuthorizationMiddlewareResultHandler
		: IAuthorizationMiddlewareResultHandler
{
	private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

	public async Task HandleAsync(
			RequestDelegate next,
			HttpContext context,
			AuthorizationPolicy policy,
			PolicyAuthorizationResult authorizeResult)
	{
		if (authorizeResult.Challenged)
		{
			context.Response.StatusCode = StatusCodes.Status401Unauthorized;
			context.Response.ContentType = "application/json";

			await context.Response.WriteAsJsonAsync(new
			{
				status = 401,
				title = "Unauthorized",
				detail = "Authentication is required to access this resource."
			});

			return;
		}
		if (authorizeResult.Forbidden)
		{
			context.Response.StatusCode = StatusCodes.Status403Forbidden;
			context.Response.ContentType = "application/json";

			await context.Response.WriteAsJsonAsync(new
			{
				status = 403,
				title = "Forbidden",
				detail = "You do not have permission to perform this action."
			});

			return;
		}

		await _defaultHandler.HandleAsync(
				next,
				context,
				policy,
				authorizeResult);
	}
}