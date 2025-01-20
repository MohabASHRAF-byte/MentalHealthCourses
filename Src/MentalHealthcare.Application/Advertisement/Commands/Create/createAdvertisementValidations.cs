using FluentValidation;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Application.Advertisement.Commands.Create;

public class CreateAdvertisementValidations : AbstractValidator<CreateAdvertisementCommand>
{

    public CreateAdvertisementValidations(ILocalizationService localizationService)
    {

        RuleFor(x => x.AdvertisementName)
            .Length(1, Global.AdvertisementNameMaxLength)
            .WithMessage(
                string.Format(
                    localizationService.GetMessage("AdNameLength", "Advertisement name length must be between {0} and {1}"),
                    localizationService.TranslateNumber(1),
                    localizationService.TranslateNumber(Global.AdvertisementNameMaxLength)
                )
            );

        RuleFor(x => x.AdvertisementDescription)
            .Length(1, Global.AdvertisementDescriptionMaxLength)
            .WithMessage(
                string.Format(
                    localizationService.GetMessage("AdDescriptionLength", "Advertisement description length must be between {0} and {1} characters."),
                    localizationService.TranslateNumber(1),
                    localizationService.TranslateNumber(Global.AdvertisementDescriptionMaxLength)
                )
            );

        RuleFor(x => x.Images)
            .NotEmpty()
            .WithMessage(
                localizationService.GetMessage("AdMustHaveImage", "Advertisement must have at least one image.")
            );
    }
}