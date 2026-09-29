using FluentValidation;

namespace Booking.Application.DTOs.Hotels;

public class CreateHotelRequestValidator : AbstractValidator<CreateHotelRequest>
{
    public CreateHotelRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(3, 200);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(2000);

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(300);

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(100);

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required.")
            .MaximumLength(100);

        RuleFor(x => x.StarRating)
            .InclusiveBetween(1, 5).WithMessage("Star rating must be between 1 and 5.");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");

        RuleFor(x => x.CheckInTime)
            .LessThan(x => x.CheckOutTime)
            .When(x => x.CheckInTime.HasValue && x.CheckOutTime.HasValue)
            .WithMessage("Check-in time must be before check-out time.");
    }
}