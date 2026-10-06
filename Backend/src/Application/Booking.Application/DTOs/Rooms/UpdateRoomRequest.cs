using Booking.Domain.Enums;

namespace Booking.Application.DTOs.Rooms;

public class UpdateRoomRequest : RoomRequestBase
{
	// إلزامي في PUT حتى لا تتغير الحالة بصمت إذا نسي العميل إرسالها
	public required bool IsAvailable { get; set; }
}