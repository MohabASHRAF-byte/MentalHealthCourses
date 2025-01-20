using FluentValidation;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.Courses.Course.Commands.AddIcon;

public class AddCourseIconCommandValidator : AbstractValidator<AddCourseIconCommand>
{
    public AddCourseIconCommandValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.File)
            .CustomIsValidIcon(localizationService);
    }
}