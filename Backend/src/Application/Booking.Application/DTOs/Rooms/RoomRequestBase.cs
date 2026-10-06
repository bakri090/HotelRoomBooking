using Booking.Domain.Enums;

namespace Booking.Application.DTOs.Rooms;
public abstract class RoomRequestBase
{
	public string RoomNumber { get; set; } = string.Empty;
	public RoomType RoomType { get; set; }
	public decimal PricePerNight { get; set; }
	public int Capacity { get; set; }
	public string? Description { get; set; }
}