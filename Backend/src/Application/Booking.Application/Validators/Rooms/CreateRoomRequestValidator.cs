using Booking.Application.Validators.Rooms;
using FluentValidation;

namespace Booking.Application.DTOs.Rooms;

public class CreateRoomRequestValidator : RoomRequestValidatorBase<CreateRoomRequest>
{
}