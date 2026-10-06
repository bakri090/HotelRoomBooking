using Booking.Domain.Enums;

namespace Booking.Application.DTOs.Rooms;

public class RoomResponse
{
	public Guid Id { get; set; }
	public Guid HotelId { get; set; }
	public string RoomNumber { get; set; } = string.Empty;
	public RoomType RoomType { get; set; }
	public decimal PricePerNight { get; set; }
	public int Capacity { get; set; }
	public string Description { get; set; } = string.Empty;
	public bool IsAvailable { get; set; }
	public DateTime CreatedAt { get; set; }
}