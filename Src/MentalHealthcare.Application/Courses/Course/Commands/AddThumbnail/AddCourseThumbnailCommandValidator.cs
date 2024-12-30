using FluentValidation;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.Courses.Course.Commands.AddThumbnail;

public class AddCourseThumbnailCommandValidator : AbstractValidator<AddCourseThumbnailCommand>
{
    public AddCourseThumbnailCommandValidator()
    {
        RuleFor(x => x.File)
            .CustomIsValidThumbnail();
    }
}