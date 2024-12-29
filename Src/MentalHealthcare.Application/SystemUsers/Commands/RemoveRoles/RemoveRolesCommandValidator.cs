using FluentValidation;
using MentalHealthcare.Application.validations;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Application.SystemUsers.Commands.RemoveRoles;

public class RemoveRolesCommandValidator : AbstractValidator<RemoveRolesCommand>
{
    public RemoveRolesCommandValidator()
    {
        RuleFor(x => x.UserName)
            .ValidateNoHtmlIfNotNull();

        RuleFor(x => x.Roles)
            .NotEmpty().WithMessage("Roles must not be empty.")
            .Must(roles => roles.All(role => Enum.IsDefined(typeof(UserRoles), role)))
            .WithMessage("Roles contain invalid values.");
    }
}