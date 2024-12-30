using FluentValidation;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.Courses.Course.Queries.GetAll;

public class GetAllCoursesQueryValidator : AbstractValidator<GetAllCoursesQuery>
{
    public GetAllCoursesQueryValidator()
    {
        RuleFor(c => c.PageNumber)
            .CustomValidatePageNumber();
        RuleFor(c => c.PageSize)
            .CustomValidatePageSize();
        RuleFor(c => c.SearchText)
            .CustomValidateSearchTerm();
    }
}