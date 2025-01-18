using FluentValidation;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.SystemUsers.Profile.UpdateProfile;

public class UpdateProfileDataCommandValidator : AbstractValidator<UpdateProfileDataCommand>
{
    public UpdateProfileDataCommandValidator(ILocalizationService localizationService)
    {
        // FirstName validation
        RuleFor(x => x.FirstName)
            .CustomIsValidNullableName(localizationService);

        // LastName validation
        RuleFor(x => x.LastName)
            .CustomIsValidNullableName(localizationService);

        // PhoneNumber validation
        RuleFor(x => x.PhoneNumber)
            .CustomIsValidPhoneNumberIfNotNull(localizationService);

        // BirthDate validation
        RuleFor(x => x.BirthDate)
            .CustomIsValidBirthDateIfNotNull(localizationService);
    }
}