using FluentValidation;
using MentalHealthcare.Domain.Constants;
using Microsoft.AspNetCore.Http; // Ensure this is included if you're using IFormFile

namespace MentalHealthcare.Application.Authors.Commands.Create
{
    public class CreateAuthorValidations : AbstractValidator<CreateAuthorCommand>
    {
        public CreateAuthorValidations()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Author name is required.")
                .Length(1, Global.AuthorNameMaxLength)
                .WithMessage($"Author name length must be between 1 and {Global.AuthorNameMaxLength} characters.");

           

            RuleFor(x => x.About)
                .MaximumLength(Global.AuthorDescriptionMaxLength)
                .WithMessage($"About section cannot exceed {Global.AuthorDescriptionMaxLength} characters.");

            RuleFor(x => x.AddedBy)
                .NotEmpty()
                .WithMessage("AddedBy field is required.");

            RuleFor(x => x.ImageUrl) // Assuming Images is a List<IFormFile>
                .NotEmpty()
                .WithMessage("At least one image must be provided.");
                
        }

       

    }
}