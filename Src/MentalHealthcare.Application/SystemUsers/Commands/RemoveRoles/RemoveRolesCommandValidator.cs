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

    }
}