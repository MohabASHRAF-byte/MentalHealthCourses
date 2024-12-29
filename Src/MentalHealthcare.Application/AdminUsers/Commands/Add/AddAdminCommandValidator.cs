using FluentValidation;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.AdminUsers.Commands.Add;

public class AddAdminCommandValidator : AbstractValidator<AddAdminCommand>
{
    public AddAdminCommandValidator()
    {
        RuleFor(A => A.Email)
            .CustomIsValidEmail();
    }
}