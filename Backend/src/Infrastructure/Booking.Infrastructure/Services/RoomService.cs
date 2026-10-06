using Booking.Application.DTOs.Hotels;
using Booking.Application.DTOs.Rooms;
using Booking.Application.Interfaces;
using Booking.Domain.Abstractions;
using Booking.Domain.Entities;
using Booking.Domain.Errors;
using Booking.Infrastructure.Repositories;
using Mapster;

namespace Booking.Infrastructure.Services;

public class RoomService(IRoomRepository _roomRepository, IHotelRepository _hotelRepository) : IRoomService
{

	public async Task<Result<RoomResponse>> CreateAsync(Guid actorId, bool isAdmin, Guid hotelId, CreateRoomRequest request, CancellationToken cancellationToken)
	{
		var hotel = await _hotelRepository.GetByIdAsync(hotelId, cancellationToken);

		if (hotel is null)
			return Result.Failure<RoomResponse>(HotelErrors.NotFound);

		if (!isAdmin && hotel.OwnerId != actorId)
			return Result.Failure<RoomResponse>(HotelErrors.AccessDenied);

		var roomNumber = request.RoomNumber.Trim();

		if (await _roomRepository.RoomNumberExistsAsync(hotelId, roomNumber, null, cancellationToken))
			return Result.Failure<RoomResponse>(RoomErrors.DuplicateRoomNumber);

		var room = request.Adapt<Room>();
		room.HotelId = hotelId;
		
		await _roomRepository.AddAsync(room, cancellationToken);
		await _roomRepository.SaveChangesAsync(cancellationToken);

		return Result.Success(room.Adapt<RoomResponse>());
	}

	public async Task<Result<List<RoomResponse>>> GetByHotelAsync(Guid hotelId, CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetByIdAsync(hotelId, cancellationToken);

        if (hotel is null)
            return Result.Failure<List<RoomResponse>>(HotelErrors.NotFound);

        var rooms = await _roomRepository.GetActiveByHotelAsync(hotelId, cancellationToken);

        return Result.Success(rooms.Adapt<List<RoomResponse>>());
    }

    public async Task<Result<RoomResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByIdAsync(id, cancellationToken);

        return room is null || !room.IsActive
            ? Result.Failure<RoomResponse>(RoomErrors.NotFound)
            : Result.Success(room.Adapt<RoomResponse>());
    }

    public async Task<Result<RoomResponse>> UpdateAsync(Guid actorId, bool isAdmin, Guid id, UpdateRoomRequest request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByIdAsync(id, cancellationToken);

        if (room is null)
            return Result.Failure<RoomResponse>(RoomErrors.NotFound);

        var hotel = await _hotelRepository.GetByIdAsync(room.HotelId, cancellationToken);

        if (hotel is null)
            return Result.Failure<RoomResponse>(HotelErrors.NotFound);

        if (!isAdmin && hotel.OwnerId != actorId)
            return Result.Failure<RoomResponse>(HotelErrors.AccessDenied);

        request.Adapt(room);

        await _roomRepository.SaveChangesAsync(cancellationToken);

        return Result.Success(room.Adapt<RoomResponse>());
    }

    public async Task<Result<RoomResponse>> DeactivateAsync(Guid actorId, bool isAdmin, Guid id, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByIdAsync(id, cancellationToken);

        if (room is null)
            return Result.Failure<RoomResponse>(RoomErrors.NotFound);

        var hotel = await _hotelRepository.GetByIdAsync(room.HotelId, cancellationToken);

        if (hotel is null)
            return Result.Failure<RoomResponse>(HotelErrors.NotFound);

        if (!isAdmin && hotel.OwnerId != actorId)
            return Result.Failure<RoomResponse>(HotelErrors.AccessDenied);

        room.IsActive = false;

        await _roomRepository.SaveChangesAsync(cancellationToken);

        return Result.Success(room.Adapt<RoomResponse>());
    }
	public async Task<Result> ToggleActivationAsync(Guid actorId, bool isAdmin, Guid id, CancellationToken cancellationToken)
	{
		var room = await _roomRepository.GetByIdAsync(id, cancellationToken);
		
    if (room is null)
			return Result.Failure<RoomResponse>(RoomErrors.NotFound);

		var hotel = await _hotelRepository.GetByIdAsync(room.HotelId, cancellationToken);

		if (hotel is null)
			return Result.Failure(HotelErrors.NotFound);

		if (!isAdmin && hotel.OwnerId != actorId)
			return Result.Failure(HotelErrors.AccessDenied);

		room.IsActive = !room.IsActive;

		await _roomRepository.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}
}