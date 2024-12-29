using FluentValidation;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.SystemUsers.Commands.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email must not be empty.")
            .EmailAddress().WithMessage("Email must be a valid email address.")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password must not be empty.")
            .MinimumLength(7).WithMessage("New password must be at least 7 characters long.")
            .MaximumLength(50).WithMessage("New password must not exceed 50 characters.");
  }
}