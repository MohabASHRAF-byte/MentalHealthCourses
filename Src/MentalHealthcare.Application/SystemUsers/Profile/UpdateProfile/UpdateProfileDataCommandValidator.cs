using FluentValidation;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.SystemUsers.Profile.UpdateProfile;

public class UpdateProfileDataCommandValidator : AbstractValidator<UpdateProfileDataCommand>
{
    public UpdateProfileDataCommandValidator()
    {
        // FirstName validation
        RuleFor(x => x.FirstName)
            .CustomIsValidNameIfNotNull();

        // LastName validation
        RuleFor(x => x.LastName)
            .CustomIsValidNameIfNotNull();

        // PhoneNumber validation
        RuleFor(x => x.PhoneNumber)
            .CustomIsValidPhoneNumberIfNotNull();

        // BirthDate validation
        RuleFor(x => x.BirthDate)
            .CustomIsValidBirthDateIfNotNull();
    }
}