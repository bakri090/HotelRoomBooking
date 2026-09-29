using FluentValidation;

namespace Booking.Application.DTOs.Rooms;

public class CreateRoomRequestValidator : AbstractValidator<CreateRoomRequest>
{
    public CreateRoomRequestValidator()
    {
        RuleFor(x => x.RoomNumber)
            .NotEmpty().WithMessage("Room number is required.")
            .MaximumLength(20);

        RuleFor(x => x.RoomType)
            .IsInEnum().WithMessage("Invalid room type.");

        RuleFor(x => x.PricePerNight)
            .GreaterThan(0).WithMessage("Price per night must be greater than zero.");

        RuleFor(x => x.Capacity)
            .GreaterThan(0).WithMessage("Capacity must be greater than zero.");

        RuleFor(x => x.Description)
            .MaximumLength(1000);
    }
}