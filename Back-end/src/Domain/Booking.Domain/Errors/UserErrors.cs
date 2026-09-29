using Booking.Domain.Abstractions;

namespace Booking.Domain.Errors;

public static class UserErrors
{
	public static readonly Error InvalidCredentials = new("User.InvalidCredentials", "Invalid email/password", StatusCodes.Status401Unauthorized);
	public static readonly Error InvalidToken = new("User.InvalidToken", "invalid token", StatusCodes.Status401Unauthorized);
	public static readonly Error OperationFailed = new("User.OperationFailed", "Operation failed", StatusCodes.Status401Unauthorized);
	public static readonly Error DuplicatedEmail = new("User.DuplicatedEmail", "Anthor user with the same email is already exists", StatusCodes.Status409Conflict);
}