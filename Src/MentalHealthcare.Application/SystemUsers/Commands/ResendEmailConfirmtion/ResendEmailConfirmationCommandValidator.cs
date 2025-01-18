using FluentValidation;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.SystemUsers.Commands.ResendEmailConfirmtion;

public class ResendEmailConfirmationCommandValidator:AbstractValidator<ResendEmailConfirmationCommand>
{
    public ResendEmailConfirmationCommandValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.Email)
            .CustomIsValidEmail(localizationService);
    }
}