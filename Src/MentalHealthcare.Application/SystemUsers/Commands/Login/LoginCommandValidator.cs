using FluentValidation;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.SystemUsers.Commands.Login;

public class LoginCommandValidator: AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.UserIdentifier)
            .CustomIsValidUsername();
        RuleFor(x => x.Otp)
            .ValidateDigitsOnlyIfNotNull();

    }
}