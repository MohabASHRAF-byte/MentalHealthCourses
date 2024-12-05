using MediatR;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.HelpCenterItem.Queries;

public class GetHelpCenterItemQueryHandler(
    ILogger<GetHelpCenterItemQueryHandler> logger,
    IHelpCenterRepository helpCenterRepository
    ):IRequestHandler<GetHelpCenterItemQuery,List<Domain.Entities.HelpCenterItem>>
{
    public async Task<List<Domain.Entities.HelpCenterItem>> Handle(GetHelpCenterItemQuery request, CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(typeof(Global.HelpCenterItems), request.itemType))
        {
            logger.LogWarning("Invalid HelpCenterItemType: @{type}", request.itemType);
            throw new ArgumentException($"Invalid value for {nameof(request.itemType)}: {request.itemType}");
        }
        logger.LogInformation("Handling GetTermsAndConditionsQuery");
        var terms = await helpCenterRepository.GetAllAsync(request.itemType);
        return terms;
    }
}