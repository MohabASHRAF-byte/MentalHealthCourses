using FluentValidation;
using MentalHealthcare.Application.validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Instructors.Commands.Create
{
    public class CreateInstructorCommandValidator : AbstractValidator<CreateInstructorCommand>
    {


        public CreateInstructorCommandValidator()
        {


            RuleFor(x => x.Name)
                .CustomIsValidName();


            RuleFor(x => x.Name)
                .CustomIsValidAbout();









        }









    }
}