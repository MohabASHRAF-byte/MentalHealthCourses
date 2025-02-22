using FluentValidation;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.Author.Commands.Create;

public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
{
    public CreateAuthorCommandValidator()
    {
        RuleFor(x => x.Name)
            .CustomIsValidName();


        RuleFor(x => x.Name)
            .CustomIsValidAbout();
    }
    
}