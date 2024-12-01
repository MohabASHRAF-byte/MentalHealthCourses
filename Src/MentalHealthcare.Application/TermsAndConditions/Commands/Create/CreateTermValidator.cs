using FluentValidation;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Application.TermsAndConditions.Commands.Create;

public class CreateTermValidator:AbstractValidator<CreateTermCommand>
{
    public CreateTermValidator()
    {
        RuleFor(x => x.Name)
            .Length(1,Global.TermNameMaxLength)
            .WithMessage($"Name must be between 1 and {Global.TermNameMaxLength} characters.");
        RuleFor(x => x.Description)
            .Length(1, Global.TermDescriptionMaxLength)
            .WithMessage($"Description must be between 1 and {Global.TermDescriptionMaxLength} characters.");
    }
}

