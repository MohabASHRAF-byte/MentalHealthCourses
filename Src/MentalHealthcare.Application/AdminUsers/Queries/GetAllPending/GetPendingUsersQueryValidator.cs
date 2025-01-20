using FluentValidation;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.AdminUsers.Queries.GetAllPending;

public class GetPendingUsersQueryValidator : AbstractValidator<GetPendingUsersQuery>
{
    public GetPendingUsersQueryValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.PageNumber) // Use a tuple instead of an anonymous type
            .CustomValidatePageNumber(localizationService);
        RuleFor(x => x.PageSize)
            .CustomValidatePageSize(localizationService);
        RuleFor(x => x.SearchText)
            .CustomValidateSearchTerm(localizationService);
    }
}