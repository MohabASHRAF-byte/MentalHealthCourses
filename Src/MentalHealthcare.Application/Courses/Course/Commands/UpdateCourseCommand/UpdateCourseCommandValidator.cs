using FluentValidation;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.Courses.Course.Commands.UpdateCourseCommand;

public class UpdateCourseCommandValidator:AbstractValidator<UpdateCourseCommand>
{
    public UpdateCourseCommandValidator()
    {
        RuleFor(x => x.Name)
            .CustomIsValidNullableName();
        RuleFor(x => x.InstructorId)
            .CustomValidateNullableId();
        RuleForEach(x => x.CategoryId)
            .CustomValidateId();
        RuleFor(x => x.Price)
            .CustomValidateNullablePrice();
        
        RuleFor(x => x.Description)
            .Must(description => description == null || description.Length <= 800)
            .WithMessage("Course description must be no more than 800 characters.")
            .When(x => x.Description != null)!
            .ValidateNoHtmlNotNull();
    }
}