using FluentValidation;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.Courses.Course.Commands.UpdateCourseCommand;

public class UpdateCourseCommandValidator:AbstractValidator<UpdateCourseCommand>
{
    public UpdateCourseCommandValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.Name)
            .CustomIsValidNullableName(localizationService);
        RuleFor(x => x.Price)
           .CustomValidateNullablePrice(localizationService);
        //
        RuleFor(x => x.Description)
            .Must(description => description == null || description.Length <= 800)
            .WithMessage("Course description must be no more than 800 characters.");
    }
}