using Booking.Application.DTOs.Hotels;
using Booking.Domain.Abstractions;

namespace Booking.Application.Interfaces;

public interface IHotelService
{
    Task<Result<HotelResponse>> CreateAsync(Guid ownerId, CreateHotelRequest request, CancellationToken cancellationToken);
    Task<Result<HotelListResponse>> GetAllAsync(GetHotelsRequest request, CancellationToken cancellationToken);
    Task<Result<HotelResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<HotelResponse>> UpdateAsync(Guid actorId, bool isAdmin, Guid id, UpdateHotelRequest request, CancellationToken cancellationToken);
    
	Task<Result> ToggleActivationAsync(Guid actorId, bool isAdmin, Guid id, CancellationToken cancellationToken);
}