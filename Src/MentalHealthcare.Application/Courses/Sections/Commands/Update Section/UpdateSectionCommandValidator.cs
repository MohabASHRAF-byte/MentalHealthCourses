using FluentValidation;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.Courses.Sections.Commands.Update_Section;

public class UpdateSectionCommandValidator:AbstractValidator<UpdateSectionCommand>
{
    public UpdateSectionCommandValidator()
    {
        RuleFor(x => x.SectionName)
            .CustomIsValidName();
    }
}