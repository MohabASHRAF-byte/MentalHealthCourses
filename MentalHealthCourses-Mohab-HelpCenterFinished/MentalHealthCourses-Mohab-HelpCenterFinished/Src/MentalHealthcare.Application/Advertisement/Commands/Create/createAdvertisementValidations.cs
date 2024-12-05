using FluentValidation;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Application.Advertisement.Commands.Create;

public class CreateAdvertisementValidations : AbstractValidator<CreateAdvertisementCommand>
{
    public CreateAdvertisementValidations()
    {
        RuleFor(x => x.AdvertisementName)
            .Length(1, Global.AdvertisementNameMaxLength)
            .WithMessage($"Advertisement name length must be between 1 and {Global.AdvertisementNameMaxLength}");
        RuleFor(x => x.AdvertisementDescription)
            .Length(1, Global.AdvertisementDescriptionMaxLength)
            .WithMessage(
                $"Advertisement description length must be between 1 and {Global.AdvertisementDescriptionMaxLength} characters.");
        RuleFor(x => x.Images)
            .NotEmpty()
            .WithMessage($"Advertisement Must have at least one image.");
    }
}