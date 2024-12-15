using FluentValidation;

namespace MentalHealthcare.Application.SystemUsers.Commands.ConfirmEmail;

public class ConfirmEmailValidator:AbstractValidator<ConfirmEmailCommand>
{

    public ConfirmEmailValidator()
    {
        RuleFor(c => c.Email)
            .EmailAddress()
            .WithMessage("Invalid email address");
        RuleFor(c => c.Email)
            .Length(3,100);
    }
}