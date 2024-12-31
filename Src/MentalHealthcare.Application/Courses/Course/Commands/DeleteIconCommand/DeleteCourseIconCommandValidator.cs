using FluentValidation;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.Courses.Course.Commands.DeleteIconCommand;

public class DeleteCourseIconCommandValidator:AbstractValidator<DeleteCourseIconCommand>
{
    public DeleteCourseIconCommandValidator()
    {
        RuleFor(x => x.CourseId)
            .CustomValidateId();
    }
}