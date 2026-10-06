using Booking.Application.DTOs.Auth;
using Booking.Application.DTOs.Hotels;
using Booking.Application.DTOs.Rooms;
using Booking.Domain.Entities;
using Mapster;

namespace Booking.Application.Mapping;

public class MappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		//config.NewConfig<ApplicationUser, AuthResponse>

		config.NewConfig<Hotel, HotelResponse>();
		config.NewConfig<CreateHotelRequest, Hotel>();

		config.NewConfig<UpdateHotelRequest, Hotel>()
				.Ignore(d => d.Id)
				.Ignore(d => d.OwnerId)
				.Ignore(d => d.IsActive)
				.Ignore(d => d.CreatedAt);

		config.NewConfig<Room, RoomResponse>()
				.Map(d => d.Description, s => s.Description ?? string.Empty);

		config.NewConfig<CreateRoomRequest, Room>()
				.Map(d => d.Description, s => s.Description ?? string.Empty)
				.Ignore(d => d.Id)
				.Ignore(d => d.HotelId)
				.Ignore(d => d.Hotel)
				.Ignore(d => d.IsActive)
				.Ignore(d => d.CreatedAt)
				.Ignore(d => d.UpdatedAt);

		config.NewConfig<UpdateRoomRequest, Room>()
				.Map(d => d.Description, s => s.Description ?? string.Empty)
				.Ignore(d => d.Id)
				.Ignore(d => d.HotelId)
				.Ignore(d => d.Hotel)
				.Ignore(d => d.IsActive)
				.Ignore(d => d.CreatedAt)
				.Ignore(d => d.UpdatedAt);
	}
}
