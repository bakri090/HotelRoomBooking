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
		config.NewConfig<UpdateHotelRequest, Hotel>();

		config.NewConfig<Room, RoomResponse>();
		config.NewConfig<CreateRoomRequest, Room>()
			.Map(dest => dest.Description, src => src.Description ?? string.Empty);
		config.NewConfig<UpdateRoomRequest, Room>()
			.Map(dest => dest.Description, src => src.Description ?? string.Empty);

		TypeAdapterConfig<UpdateHotelRequest, Hotel>
				.NewConfig()
				.Ignore(dest => dest.Id)
				.Ignore(dest => dest.OwnerId)
				.Ignore(dest => dest.IsActive)
				.Ignore(dest => dest.CreatedAt);
	}
}