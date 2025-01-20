using FluentValidation;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.SystemUsers.Commands.Login;

public class LoginCommandValidator: AbstractValidator<LoginCommand>
{
    public LoginCommandValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.UserIdentifier)
            .CustomIsValidUsername(localizationService);
        RuleFor(x => x.Otp)
            .ValidateDigitsOnlyIfNotNull(localizationService);

    }
}