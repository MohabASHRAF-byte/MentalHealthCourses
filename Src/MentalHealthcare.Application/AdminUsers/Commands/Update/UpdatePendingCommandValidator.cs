using FluentValidation;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.AdminUsers.Commands.Update;

public class UpdatePendingCommandValidator : AbstractValidator<UpdatePendingAdminCommand>
{
    public UpdatePendingCommandValidator()
    {
        RuleFor(up => up.NewEmail)
            .CustomIsValidEmail();

        RuleFor(up => up.OldEmail)
            .CustomIsValidEmail();
    }
}