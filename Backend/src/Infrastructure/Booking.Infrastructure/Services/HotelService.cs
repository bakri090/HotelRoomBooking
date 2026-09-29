using Booking.Application.DTOs.Hotels;
using Booking.Application.Interfaces;
using Booking.Domain.Abstractions;
using Booking.Domain.Entities;
using Booking.Domain.Errors;
using Mapster;

namespace Booking.Infrastructure.Services;

public class HotelService : IHotelService
{
    private readonly IHotelRepository _hotelRepository;

    public HotelService(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public async Task<Result<HotelResponse>> CreateAsync(Guid ownerId, CreateHotelRequest request, CancellationToken cancellationToken)
    {
        var hotel = request.Adapt<Hotel>();
        hotel.OwnerId = ownerId;

        await _hotelRepository.AddAsync(hotel, cancellationToken);
        await _hotelRepository.SaveChangesAsync(cancellationToken);

        return Result.Success(hotel.Adapt<HotelResponse>());
    }

    public async Task<Result<HotelListResponse>> GetAllAsync(GetHotelsRequest request, CancellationToken cancellationToken)
    {
        var items = await _hotelRepository.GetActiveAsync(
            request.City, request.Country, request.StarRating,
            request.PageNumber, request.PageSize, cancellationToken);

        var totalCount = await _hotelRepository.CountActiveAsync(
            request.City, request.Country, request.StarRating, cancellationToken);

        return Result.Success(new HotelListResponse
        {
					Items = items.Select(hotel => hotel.Adapt<HotelResponse>()).ToList(),
					TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        });
    }

    public async Task<Result<HotelResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetActiveByIdAsync(id, cancellationToken);

        return hotel is null
            ? Result.Failure<HotelResponse>(HotelErrors.NotFound)
            : Result.Success(hotel.Adapt<HotelResponse>());
    }

    public async Task<Result<HotelResponse>> UpdateAsync(Guid actorId, bool isAdmin, Guid id, UpdateHotelRequest request, CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetByIdAsync(id, cancellationToken);

        if (hotel is null)
            return Result.Failure<HotelResponse>(HotelErrors.NotFound);

        if (!isAdmin && hotel.OwnerId != actorId)
            return Result.Failure<HotelResponse>(HotelErrors.AccessDenied);

        request.Adapt(hotel);

        await _hotelRepository.SaveChangesAsync(cancellationToken);

        return Result.Success(hotel.Adapt<HotelResponse>());
    }

    public async Task<Result<HotelResponse>> DeactivateAsync(Guid actorId, bool isAdmin, Guid id, CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetByIdAsync(id, cancellationToken);

        if (hotel is null)
            return Result.Failure<HotelResponse>(HotelErrors.NotFound);

        if (!isAdmin && hotel.OwnerId != actorId)
            return Result.Failure<HotelResponse>(HotelErrors.AccessDenied);

        hotel.IsActive = false;

        await _hotelRepository.SaveChangesAsync(cancellationToken);

        return Result.Success(hotel.Adapt<HotelResponse>());
    }
	public async Task<Result> ToggleActivationAsync(Guid actorId, bool isAdmin, Guid id, CancellationToken cancellationToken)
	{
		var hotel = await _hotelRepository.GetByIdAsync(id, cancellationToken);

		if (hotel is null)
			return Result.Failure(HotelErrors.NotFound);

		if (!isAdmin && hotel.OwnerId != actorId)
			return Result.Failure<HotelResponse>(HotelErrors.AccessDenied);

		hotel.IsActive = !hotel.IsActive;

		await _hotelRepository.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}
}