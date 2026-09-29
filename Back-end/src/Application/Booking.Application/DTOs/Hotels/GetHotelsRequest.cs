namespace Booking.Application.DTOs.Hotels;

public class GetHotelsRequest
{
    public string? City { get; set; }
    public string? Country { get; set; }
    public int? StarRating { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}