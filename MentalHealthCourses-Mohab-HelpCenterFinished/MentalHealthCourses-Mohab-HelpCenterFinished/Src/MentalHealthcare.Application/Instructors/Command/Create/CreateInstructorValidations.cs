using FluentValidation;
using MentalHealthcare.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Instructors.Command.Create
{
    public class CreateInstructorValidations : AbstractValidator<CreateInstructorCommand>
    {
        public CreateInstructorValidations()
        {
            RuleFor(x => x.Name)
            .Length(1, Global.InstructorNameMaxLength)
            .WithMessage($"Instructor name length must be between 1 and {Global.InstructorNameMaxLength}");
            RuleFor(x => x.About)
                .Length(1, Global.InstructorDescriptionMaxLength)
                .WithMessage(
                    $"Instructor description length must be between 1 and {Global.InstructorDescriptionMaxLength} characters.");
            RuleFor(x => x.ImageUrl)
                .NotEmpty()
                .WithMessage($"Instructor Must have at least one image.");
        }
    }
}
