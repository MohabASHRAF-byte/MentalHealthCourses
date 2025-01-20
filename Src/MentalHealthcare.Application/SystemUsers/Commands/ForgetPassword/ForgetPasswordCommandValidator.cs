using FluentValidation;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.SystemUsers.Commands.ForgetPassword;

public class ForgetPasswordCommandValidator : AbstractValidator<ForgetPasswordCommand>
{
    public ForgetPasswordCommandValidator(ILocalizationService localizationService)
    {

        RuleFor(x => x.Email)
            .CustomIsValidEmail(localizationService);
    }
}