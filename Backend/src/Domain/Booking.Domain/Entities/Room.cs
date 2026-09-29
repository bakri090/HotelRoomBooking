using Booking.Domain.Common;
using Booking.Domain.Enums;

namespace Booking.Domain.Entities;

public class Room : BaseEntity
{
	public Guid HotelId { get; set; }
	public string RoomNumber { get; set; } = string.Empty;
	public RoomType RoomType { get; set; }
	public decimal PricePerNight { get; set; }
	public int Capacity { get; set; }
	public string Description { get; set; } = string.Empty;
	public bool IsAvailable { get; set; } = true;

	public Hotel Hotel { get; set; } = null!;
}