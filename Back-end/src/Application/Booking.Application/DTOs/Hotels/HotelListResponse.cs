namespace Booking.Application.DTOs.Hotels;

public class HotelListResponse
{
    public List<HotelResponse> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}