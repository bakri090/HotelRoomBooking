using Booking.Domain.Entities;

namespace Booking.Application.Interfaces;

public interface IRoomRepository
{
  Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
	Task<Room?> GetByIdWithHotelAsync(Guid id, CancellationToken cancellationToken);
	Task<List<Room>> GetActiveByHotelAsync(Guid hotelId, CancellationToken cancellationToken);
	Task<bool> RoomNumberExistsAsync(Guid hotelId, string roomNumber, Guid? excludeRoomId, CancellationToken cancellationToken);
	Task AddAsync(Room room, CancellationToken cancellationToken);
	Task SaveChangesAsync(CancellationToken cancellationToken);
}