using FluentValidation;
using MentalHealthcare.Application.Instructors.Commands.Create;
using MentalHealthcare.Application.Resources.Localization.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Meditations.Command.Create
{
    public class CreateMeditationCommandValidator : AbstractValidator<CreateInstructorCommand>
    {

        public CreateMeditationCommandValidator(ILocalizationService localizationService)
        {


            // RuleFor(x => x.Name)
            //     .CustomIsValidName(localizationService);


            // RuleFor(x => x.Name)
            //     .CustomIsValidAbout();









        }





    }
}
