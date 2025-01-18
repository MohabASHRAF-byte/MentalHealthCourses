using FluentValidation;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.Courses.Course.Commands.Create;

public class CreateCourseCommandValidator: AbstractValidator<CreateCourseCommand>
{
  public CreateCourseCommandValidator(ILocalizationService localizationService)
  {
    RuleFor(x => x.Name)
      .CustomIsValidName(localizationService);
    RuleFor(x => x.InstructorId)
      .CustomValidateId(localizationService);
    RuleForEach(x => x.CategoryId)
      .CustomValidateId(localizationService);
    RuleFor(x => x.Price)
      .CustomValidatePrice(localizationService);
    RuleFor(x => x.Description)
      .ValidateNoHtmlNotNull(localizationService)
      .MaximumLength(800)
      .WithMessage("Course description must be no more than 800 characters");
    
  }
}
