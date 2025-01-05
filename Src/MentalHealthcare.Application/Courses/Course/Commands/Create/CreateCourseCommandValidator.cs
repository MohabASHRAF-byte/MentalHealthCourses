using FluentValidation;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.Courses.Course.Commands.Create;

public class CreateCourseCommandValidator: AbstractValidator<CreateCourseCommand>
{
  public CreateCourseCommandValidator()
  {
    RuleFor(x => x.Name)
      .CustomIsValidName();
    RuleFor(x => x.InstructorId)
      .CustomValidateId();
    RuleForEach(x => x.CategoryId)
      .CustomValidateId();
    RuleFor(x => x.Price)
      .CustomValidatePrice();
    RuleFor(x => x.Description)
      .ValidateNoHtmlNotNull()
      .MaximumLength(800)
      .WithMessage("Course description must be no more than 800 characters");
    
  }
}
