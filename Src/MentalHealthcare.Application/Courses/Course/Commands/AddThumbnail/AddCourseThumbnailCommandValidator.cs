using FluentValidation;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.Courses.Course.Commands.AddThumbnail;

public class AddCourseThumbnailCommandValidator : AbstractValidator<AddCourseThumbnailCommand>
{
    public AddCourseThumbnailCommandValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.File)
            .CustomIsValidThumbnail(localizationService);
    }
}