using FluentValidation;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.SystemUsers.Commands.ConfirmEmail;

public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailValidator(ILocalizationService localizationService)
    {
        RuleFor(c => c.Email)
            .CustomIsValidEmail(localizationService);
    }
}