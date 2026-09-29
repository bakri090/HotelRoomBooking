using Booking.Domain.Entities;

namespace Booking.Application.Interfaces;

public interface IRoomRepository
{
    Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Room>> GetActiveByHotelAsync(Guid hotelId, CancellationToken cancellationToken);
    Task AddAsync(Room room, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}