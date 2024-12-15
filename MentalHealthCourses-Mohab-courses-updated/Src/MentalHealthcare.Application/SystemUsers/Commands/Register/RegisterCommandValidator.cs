using FluentValidation;

namespace MentalHealthcare.Application.SystemUsers.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(register => register.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .Matches(@"^[a-zA-Z0-9]+$").WithMessage("Username can only contain letters and numbers, no special characters.")
                .Matches(@"[a-zA-Z]").WithMessage("Username must contain at least one letter.");
        }
    }
}