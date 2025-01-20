using FluentValidation;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.Courses.Sections.Commands.Add_Section;

public class AddCourseSectionCommandValidator: AbstractValidator<AddCourseSectionCommand>
{
    public AddCourseSectionCommandValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.Name)
            .CustomIsValidName(localizationService);

    }
}