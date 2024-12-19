using FluentValidation;

namespace MentalHealthcare.Application.PromoCode.Course.Commands.AddCoursePromoCode;

public class AddCoursePromoCodeValidator: AbstractValidator<AddCoursePromoCodeCommand>
{
    public AddCoursePromoCodeValidator()
    {
        RuleFor(x => x.ExpireDate)
            .Must(BeAValidDateTime)
            .WithMessage("The string must be a valid DateTime in the correct format.");
    }

    private bool BeAValidDateTime(string dateString)
    {
        return DateTime.TryParse(dateString, out _);
    }
}
