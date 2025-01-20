using FluentValidation;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.AdminUsers.Commands.Add;

public class AddAdminCommandValidator : AbstractValidator<AddAdminCommand>
{
    public AddAdminCommandValidator(ILocalizationService localizationService)
    {
        RuleFor(A => A.Email)
            .CustomIsValidEmail(localizationService);
    }
}