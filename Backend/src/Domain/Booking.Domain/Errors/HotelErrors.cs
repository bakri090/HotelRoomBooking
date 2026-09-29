using Booking.Domain.Abstractions;

namespace Booking.Domain.Errors;

public static class HotelErrors
{
	public static readonly Error NotFound = new("Hotel.NotFound", "Hotel not found.", StatusCodes.Status404NotFound);
	public static readonly Error AccessDenied = new("Hotel.AccessDenied", "You do not have permission to perform this action.", StatusCodes.Status403Forbidden);
	public static readonly Error OperationFailed = new("Hotel.OperationFailed", "Hotel operation failed.", StatusCodes.Status400BadRequest);
}