using FluentValidation;
using MentalHealthcare.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Authors.Commands.Create
{
    public class createAuthorValidations : AbstractValidator<CreateAuthorCommand>
    {
        public createAuthorValidations()
        {
            RuleFor(x => x.Name)
           .Length(1, Global.AuthorNameMaxLength)
           .WithMessage($"Author name length must be between 1 and {Global.AuthorNameMaxLength}");
            RuleFor(x => x.About)
                .Length(1, Global.AuthorAboutMaxLength)
                .WithMessage(
                    $"Author About length must be between 1 and {Global.AuthorAboutMaxLength} characters.");
            RuleFor(x => x.ImageUrl)
                .NotEmpty()
                .WithMessage($"Author Must have at least one image.");
        }
    }
}
