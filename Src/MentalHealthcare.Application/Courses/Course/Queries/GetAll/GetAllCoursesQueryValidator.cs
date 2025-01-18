using FluentValidation;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.Courses.Course.Queries.GetAll;

public class GetAllCoursesQueryValidator : AbstractValidator<GetAllCoursesQuery>
{
    public GetAllCoursesQueryValidator(ILocalizationService localizationService)
    {
        RuleFor(c => c.PageNumber)
            .CustomValidatePageNumber(localizationService);
        RuleFor(c => c.PageSize)
            .CustomValidatePageSize(localizationService);
        RuleFor(c => c.SearchText)
            .CustomValidateSearchTerm(localizationService);
    }
}