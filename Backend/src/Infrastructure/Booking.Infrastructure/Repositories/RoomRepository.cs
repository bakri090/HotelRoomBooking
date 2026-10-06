using Booking.Application.Interfaces;
using Booking.Domain.Entities;
using Booking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrastructure.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly ApplicationDbContext _dbContext;

    public RoomRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Rooms
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }
	public async Task<Room?> GetByIdWithHotelAsync(Guid id, CancellationToken cancellationToken)
	{
		return await _dbContext.Rooms.Include(x =>x.Hotel)
				.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
	}
	public async Task<List<Room>> GetActiveByHotelAsync(Guid hotelId, CancellationToken cancellationToken)
    {
        return await _dbContext.Rooms
            .AsNoTracking()
            .Where(r => r.HotelId == hotelId && r.IsActive)
            .OrderBy(r => r.RoomNumber)
            .ToListAsync(cancellationToken);
    }
	public Task<bool> RoomNumberExistsAsync(Guid hotelId, string roomNumber, Guid? excludeRoomId, CancellationToken cancellationToken)
			 => _dbContext.Rooms.AnyAsync(r =>
					 r.HotelId == hotelId &&
					 r.RoomNumber == roomNumber &&
					 r.IsActive &&
					 (excludeRoomId == null || r.Id != excludeRoomId),
					 cancellationToken);
	public async Task AddAsync(Room room, CancellationToken cancellationToken)
    {
        await _dbContext.Rooms.AddAsync(room);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => _dbContext.SaveChangesAsync(cancellationToken);
}