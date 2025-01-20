using FluentValidation;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.AdminUsers.Commands.Delete;

public class DeletePendingUsersCommandValidator : AbstractValidator<DeletePendingUsersCommand>
{
    public DeletePendingUsersCommandValidator(ILocalizationService localizationService)
    {
        RuleForEach(x => x.PendingUsers)
            .CustomIsValidEmail(localizationService);
    }
}