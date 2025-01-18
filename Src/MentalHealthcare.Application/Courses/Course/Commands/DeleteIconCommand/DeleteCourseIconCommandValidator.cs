using FluentValidation;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.Courses.Course.Commands.DeleteIconCommand;

public class DeleteCourseIconCommandValidator:AbstractValidator<DeleteCourseIconCommand>
{
    public DeleteCourseIconCommandValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.CourseId)
            .CustomValidateId(localizationService);
    }
}