using FluentValidation;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.Courses.Course.Commands.DeleteThumbnail;

public class DeleteCourseThumbnailCommandValidator: AbstractValidator<DeleteCourseThumbnailCommand>
{
    public DeleteCourseThumbnailCommandValidator()
    {
        RuleFor(x => x.CourseId)
            .CustomValidateId();
    }
}