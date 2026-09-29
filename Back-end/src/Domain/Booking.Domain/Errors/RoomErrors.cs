using Booking.Domain.Abstractions;

namespace Booking.Domain.Errors;

public static class RoomErrors
{
	public static readonly Error NotFound = new("Room.NotFound", "Room not found.", StatusCodes.Status404NotFound);
	public static readonly Error AccessDenied = new("Room.AccessDenied", "You do not have permission to perform this action.", StatusCodes.Status403Forbidden);
	public static readonly Error OperationFailed = new("Room.OperationFailed", "Room operation failed.", StatusCodes.Status400BadRequest);
}