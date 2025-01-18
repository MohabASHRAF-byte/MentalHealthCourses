using FluentValidation;
using MentalHealthcare.Application.validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Instructors.Commands.AddPhoto
{
    public class AddPhotoCommandValidator : AbstractValidator<AddPhotoCommand>
    {
        public AddPhotoCommandValidator()
        {
            RuleFor(x => x.File)
            .CustomIsValidThumbnail();
        }
    }
}
