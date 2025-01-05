using FluentValidation;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.AdminUsers.Queries.GetAllPending;

public class GetPendingUsersQueryValidator : AbstractValidator<GetPendingUsersQuery>
{
    public GetPendingUsersQueryValidator()
    {
        RuleFor(x => x.PageNumber) // Use a tuple instead of an anonymous type
            .CustomValidatePageNumber();
        RuleFor(x => x.PageSize)
            .CustomValidatePageSize();
        RuleFor(x => x.SearchText)
            .CustomValidateSearchTerm();
    }
}