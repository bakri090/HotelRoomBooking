using Booking.Domain.Enums;

namespace Booking.Application.DTOs.Rooms;

public class CreateRoomRequest : RoomRequestBase
{
	public bool IsAvailable { get; set; } = true;
}