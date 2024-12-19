using FluentValidation;

namespace MentalHealthcare.Application.PromoCode.General.Commands.UpdateGeneralPromoCode;

public class UpdateGeneralPromoCodeCommandValidator: AbstractValidator<UpdateGeneralPromoCodeCommand>
{
    public UpdateGeneralPromoCodeCommandValidator()
    {
        RuleFor(x => x.ExpireDate)
            .Must(BeAValidDateTime)
            .When(x => !string.IsNullOrEmpty(x.ExpireDate))
            .WithMessage("The expiration date must be a valid DateTime in the correct format.");

        RuleFor(x => x.ExpireDate)
            .Must(BeGreaterThanCurrentTime)
            .When(x => !string.IsNullOrEmpty(x.ExpireDate))
            .WithMessage("The expiration date must be greater than the current time.");
    }

    private bool BeAValidDateTime(string? dateTimeStr)
    {
        return DateTime.TryParse(dateTimeStr, out _);
    }

    private bool BeGreaterThanCurrentTime(string? dateTimeStr)
    {
        if (DateTime.TryParse(dateTimeStr, out var dateTime))
        {
            return dateTime > DateTime.Now;
        }
        return false;
    }
}