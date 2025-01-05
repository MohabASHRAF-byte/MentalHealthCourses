using FluentValidation;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.Courses.Lessons.Commands.CreateVideo;

public class CreateVideoCommandValidator:AbstractValidator<CreateVideoCommand>
{
    public CreateVideoCommandValidator()
    {
        RuleFor(x => x.Title)
            .CustomIsValidName();
        RuleFor(x => x.LengthWithSeconds)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Length with seconds must be greater than or equal to 1.");
    }
}