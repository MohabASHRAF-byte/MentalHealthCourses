using FluentValidation;

namespace MentalHealthcare.Application.SystemUsers.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage("Old password must not be empty.")
            .MinimumLength(7).WithMessage("Old password must be at least 7 characters long.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password must not be empty.")
            .MinimumLength(7).WithMessage("New password must be at least 7 characters long.")
            .Must((command, newPassword) => !newPassword.Equals(command.OldPassword))
            .WithMessage("New password must be different from the old password.");
    }
}