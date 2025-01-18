using FluentValidation;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.AdminUsers.Commands.Update;

public class UpdatePendingCommandValidator : AbstractValidator<UpdatePendingAdminCommand>
{
    public UpdatePendingCommandValidator(ILocalizationService localizationService)
    {
        RuleFor(up => up.NewEmail)
            .CustomIsValidEmail(localizationService);

        RuleFor(up => up.OldEmail)
            .CustomIsValidEmail(localizationService);
    }
}