using FluentValidation;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.SystemUsers.Profile.UpdateProfile;

public class UpdateProfileDataCommandValidator : AbstractValidator<UpdateProfileDataCommand>
{
    public UpdateProfileDataCommandValidator()
    {
        // FirstName validation
        RuleFor(x => x.FirstName)
            .CustomIsValidNullableName();

        // LastName validation
        RuleFor(x => x.LastName)
            .CustomIsValidNullableName();

        // PhoneNumber validation
        RuleFor(x => x.PhoneNumber)
            .CustomIsValidPhoneNumberIfNotNull();

        // BirthDate validation
        RuleFor(x => x.BirthDate)
            .CustomIsValidBirthDateIfNotNull();
    }
}