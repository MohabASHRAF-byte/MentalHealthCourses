using FluentValidation;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.Courses.Course.Commands.DeleteThumbnail;

public class DeleteCourseThumbnailCommandValidator: AbstractValidator<DeleteCourseThumbnailCommand>
{
    public DeleteCourseThumbnailCommandValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.CourseId)
            .CustomValidateId(localizationService);
    }
}