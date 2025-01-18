using FluentValidation;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.Courses.Course.Queries.GetById;

public class GetCourseByIdQueryValidator : AbstractValidator<GetCourseByIdQuery>
{
    public GetCourseByIdQueryValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.Id).CustomValidateId(localizationService);
    }
}