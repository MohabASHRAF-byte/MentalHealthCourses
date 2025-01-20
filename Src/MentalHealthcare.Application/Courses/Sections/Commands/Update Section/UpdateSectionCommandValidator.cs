using FluentValidation;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.Courses.Sections.Commands.Update_Section;

public class UpdateSectionCommandValidator:AbstractValidator<UpdateSectionCommand>
{
    public UpdateSectionCommandValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.SectionName)
            .CustomIsValidName(localizationService);
    }
}