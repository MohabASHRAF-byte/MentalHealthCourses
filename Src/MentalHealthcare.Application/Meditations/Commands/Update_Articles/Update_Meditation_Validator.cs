using FluentValidation;
using MentalHealthcare.Application.Meditations.Commands.Add_Meditation;
using MentalHealthcare.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Meditations.Commands.Update_Articles
{
    internal class Update_Meditation_Validator : AbstractValidator<Update_Meditation_Command>
    {

        private readonly IMeditation _meditation;

        public Update_Meditation_Validator(IMeditation meditation)
        {
            ValidationRules();

            _meditation = meditation;
        }



        public void ValidationRules()
        {
            RuleFor(x => x.UploadedBy)
               .NotEmpty().WithMessage("Please enter your Name!")
               .NotNull();

            RuleFor(x => x.Content)
                        .NotEmpty().WithMessage("Please Attach The  Content of Meditation.")
                        .NotNull();

            #region Validator on Date
            bool BeAValidDate(DateTime date)
            {
                return !date.Equals(default(DateTime));
            }
            RuleFor(x => x.CreatedDate)
                                  .NotEmpty().WithMessage("Please Enter The  Date.")
                                  .NotNull().WithMessage("The Creation Date is a Null Value!")
                                  .Must(BeAValidDate).WithMessage("The Creation Date is not valid!");
            #endregion


            RuleFor(x => x.Title)
               .NotEmpty().WithMessage("Please enter the Title of the Meditation.");

            RuleFor(x => x.Title)
           .MustAsync(async (key, cancellation) => !await _meditation.IsExistByTitle(key))
                .WithMessage("This Title already exists.");



            RuleFor(x => x.Content)
          .MustAsync(async (key, cancellation) => !await _meditation.IsExistByTitle(key))
               .WithMessage("This Content already exists.");







        }









    }
}
