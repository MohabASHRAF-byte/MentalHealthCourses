using FluentValidation;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.SystemUsers.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(register => register.UserName)
                .ValidateNoHtmlIfNotNull();
            RuleFor(x => x.Tenant!)
                .IsAdminProgramTenant();
            RuleFor(x => x.FirstName)
                .CustomIsValidName();
            RuleFor(x => x.LastName)
                .CustomIsValidName();
            RuleFor(x => x.Email)
                .CustomIsValidEmail();
            RuleFor(x => x.PhoneNumber)
                .CustomIsValidPhoneNumber();
            RuleFor(x => x.UserName)
                .CustomIsValidUsername();
            RuleFor(x => x.Password)
                .CustomIsValidPassword()
                .Must((model, password) =>
                    !ValidationRules.ContainsPersonalInformation(password, model.FirstName, model.LastName,
                        model.UserName));
            RuleFor(x => x.Active2Fa)
                .NotNull();
        }
    }
}