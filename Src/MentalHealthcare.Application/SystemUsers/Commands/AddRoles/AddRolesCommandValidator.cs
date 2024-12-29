using FluentValidation;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Application.SystemUsers.Commands.AddRoles;

public class AddRolesCommandValidator : AbstractValidator<AddRolesCommand>
{
    public AddRolesCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("UserName must not be empty.")
            .MaximumLength(50).WithMessage("UserName must not exceed 50 characters.");

   }
}