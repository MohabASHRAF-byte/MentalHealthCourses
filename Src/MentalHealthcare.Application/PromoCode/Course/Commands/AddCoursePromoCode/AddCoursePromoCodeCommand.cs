using FluentValidation;
using MediatR;

namespace MentalHealthcare.Application.PromoCode.Course.Commands.AddCoursePromoCode;

public class AddCoursePromoCodeCommand : IRequest<int>
{
    public string Code { get; set; }

    public string ExpireDate { get; set; }

    public float Percentage { get; set; }

    public int CourseId { get; set; }
    public bool IsActive { get; set; }
}

public class AddCoursePromoCodeCommandValidator : AbstractValidator<AddCoursePromoCodeCommand>
{
    public AddCoursePromoCodeCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Promo code is required.")
            .Length(3, 50).WithMessage("Promo code must be between 3 and 50 characters.");

        RuleFor(x => x.ExpireDate)
            .NotEmpty().WithMessage("Expire date is required.")
            .Must(BeAValidFutureDate).WithMessage("Expire date must be a valid date and later than today.");

        RuleFor(x => x.Percentage)
            .GreaterThan(0).WithMessage("Percentage must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("Percentage must not exceed 100.");

        RuleFor(x => x.CourseId)
            .GreaterThan(0).WithMessage("Course ID must be a positive integer.");
    }

    private bool BeAValidFutureDate(string date)
    {
        return DateTime.TryParse(date, out var parsedDate) && parsedDate > DateTime.UtcNow;
    }
}