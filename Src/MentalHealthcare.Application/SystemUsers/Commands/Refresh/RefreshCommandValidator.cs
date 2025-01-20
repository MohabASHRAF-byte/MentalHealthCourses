using FluentValidation;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.SystemUsers.Commands.Refresh;

public class RefreshCommandValidator : AbstractValidator<RefreshCommand>
{
    public RefreshCommandValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.RefreshToken)
            .ValidateNoHtmlIfNotNull(localizationService);
    }
}