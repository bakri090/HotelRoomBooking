using FluentValidation;

namespace Booking.Application.DTOs.Hotels;

public class GetHotelsRequestValidator : AbstractValidator<GetHotelsRequest>
{
    public GetHotelsRequestValidator()
    {
        RuleFor(x => x.StarRating)
            .InclusiveBetween(1, 5).WithMessage("Star rating must be between 1 and 5.");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 50).WithMessage("Page size must be between 1 and 50.");
    }
}