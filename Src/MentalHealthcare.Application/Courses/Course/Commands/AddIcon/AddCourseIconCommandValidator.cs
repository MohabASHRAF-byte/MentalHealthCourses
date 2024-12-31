using FluentValidation;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.Courses.Course.Commands.AddIcon;

public class AddCourseIconCommandValidator : AbstractValidator<AddCourseIconCommand>
{
    public AddCourseIconCommandValidator()
    {
        RuleFor(x => x.File)
            .CustomIsValidIcon();
    }
}