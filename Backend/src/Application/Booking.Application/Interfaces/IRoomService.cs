using Booking.Application.DTOs.Rooms;
using Booking.Domain.Abstractions;

namespace Booking.Application.Interfaces;

public interface IRoomService
{
    Task<Result<RoomResponse>> CreateAsync(Guid actorId, bool isAdmin, Guid hotelId, CreateRoomRequest request, CancellationToken cancellationToken);
    Task<Result<List<RoomResponse>>> GetByHotelAsync(Guid hotelId, CancellationToken cancellationToken);
    Task<Result<RoomResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<RoomResponse>> UpdateAsync(Guid actorId, bool isAdmin, Guid id, UpdateRoomRequest request, CancellationToken cancellationToken);
    Task<Result<RoomResponse>> DeactivateAsync(Guid actorId, bool isAdmin, Guid id, CancellationToken cancellationToken);
}