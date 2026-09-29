using Booking.Domain.Enums;

namespace Booking.Application.DTOs.Rooms;

public class UpdateRoomRequest
{
    public string RoomNumber { get; set; } = string.Empty;
    public RoomType RoomType { get; set; }
    public decimal PricePerNight { get; set; }
    public int Capacity { get; set; }
    public string? Description { get; set; }
    public bool IsAvailable { get; set; } = true;
}