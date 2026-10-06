using Booking.Application.DTOs.Rooms;
using FluentValidation;

namespace Booking.Application.Validators.Rooms;

public class RoomRequestValidatorBase<T>: AbstractValidator<T> where T :RoomRequestBase
{
	protected RoomRequestValidatorBase()
	{
		RuleFor(x => x.RoomNumber)
				.NotEmpty().WithMessage("Room number is required.")
				.MaximumLength(20);

		RuleFor(x => x.RoomType)
				.IsInEnum().WithMessage("Invalid room type.");

		RuleFor(x => x.PricePerNight)
				.GreaterThan(0).WithMessage("Price per night must be greater than zero.")
				.LessThanOrEqualTo(1_000_000)
				.PrecisionScale(18, 2, true);

		RuleFor(x => x.Capacity)
				.InclusiveBetween(1, 20).WithMessage("Capacity must be between 1 and 20.");

		RuleFor(x => x.Description)
				.MaximumLength(1000);
	}
}
