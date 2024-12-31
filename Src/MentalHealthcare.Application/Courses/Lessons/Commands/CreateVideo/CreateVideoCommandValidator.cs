using FluentValidation;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.Courses.Lessons.Commands.CreateVideo;

public class CreateVideoCommandValidator:AbstractValidator<CreateVideoCommand>
{
    public CreateVideoCommandValidator()
    {
        RuleFor(x => x.Title)
            .CustomIsValidName();
        RuleFor(x => x.Description)
            .ValidateNoHtmlIfNotNull()
            .MaximumLength(800)
            .WithMessage("Course description must be no more than 800 characters");
    }
}