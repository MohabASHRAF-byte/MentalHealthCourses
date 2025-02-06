using FluentValidation;
using MentalHealthcare.Application.validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MentalHealthcare.Application.Resources.Localization.Resources;

namespace MentalHealthcare.Application.Instructors.Commands.Create
{
    public class CreateInstructorCommandValidator : AbstractValidator<CreateInstructorCommand>
    {


        public CreateInstructorCommandValidator(ILocalizationService localizationService)
        {


            RuleFor(x => x.Name)
                .CustomIsValidName(localizationService);


            RuleFor(x => x.Name)
                .CustomIsValidAbout();









        }









    }
}