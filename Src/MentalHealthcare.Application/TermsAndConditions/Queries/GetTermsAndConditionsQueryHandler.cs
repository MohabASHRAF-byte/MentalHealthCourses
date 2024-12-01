using MediatR;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.TermsAndConditions.Queries;

public class GetTermsAndConditionsQueryHandler(
    ILogger<GetTermsAndConditionsQueryHandler> logger,
    ITermsRepository termsRepository
    ):IRequestHandler<GetTermsAndConditionsQuery,List<Domain.Entities.TermsAndConditions>>
{
    public async Task<List<Domain.Entities.TermsAndConditions>> Handle(GetTermsAndConditionsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetTermsAndConditionsQuery");
        var terms = await termsRepository.GetAllAsync();
        return terms;
    }
}