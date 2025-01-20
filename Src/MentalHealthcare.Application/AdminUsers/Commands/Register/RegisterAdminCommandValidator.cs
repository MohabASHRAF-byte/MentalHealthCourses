using FluentValidation;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.AdminUsers.Commands.Register;

public class RegisterAdminCommandValidator : AbstractValidator<RegisterAdminCommand>
{
    public RegisterAdminCommandValidator(ILocalizationService localizationService)
    {

        RuleFor(x => x.FirstName)
            .CustomIsValidName(localizationService);
        RuleFor(x => x.LastName)
            .CustomIsValidName(localizationService);
        RuleFor(x => x.Email)
            .CustomIsValidEmail(localizationService);
        RuleFor(x => x.PhoneNumber)
            .CustomIsValidPhoneNumber(localizationService);
        RuleFor(x => x.UserName)
            .CustomIsValidUsername(localizationService);
        RuleFor(x => x.Password)
            .CustomIsValidPassword(localizationService)
            .Must((model, password) =>
                !ValidationRules.ContainsPersonalInformation(password, model.FirstName, model.LastName,
                    model.UserName)
                ).WithMessage("Should contain personal information");
        RuleFor(x => x.Active2Fa)
            .NotNull();
    }
}
/*
 *   [JsonIgnore]
 */