using Booking.Domain.Entities;

namespace Booking.Application.Interfaces;

public interface IHotelRepository
{
    Task<Hotel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
	Task<Hotel?> GetActiveByIdAsync(Guid id, CancellationToken cancellationToken);
	Task<List<Hotel>> GetActiveAsync(string? city, string? country, int? starRating, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<int> CountActiveAsync(string? city, string? country, int? starRating, CancellationToken cancellationToken);
    Task AddAsync(Hotel hotel, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}