using MediatR;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.HelpCenterItem.Queries;

public class GetHelpCenterItemQueryHandler(
    ILogger<GetHelpCenterItemQueryHandler> logger,
    IHelpCenterRepository helpCenterRepository,
    ILocalizationService localizationService
) : IRequestHandler<GetHelpCenterItemQuery, List<Domain.Entities.HelpCenterItem>>
{
    public async Task<List<Domain.Entities.HelpCenterItem>> Handle(GetHelpCenterItemQuery request, CancellationToken cancellationToken)
    {
        // Validate HelpCenterItemType
        logger.LogInformation("Validating HelpCenterItemType: {ItemType}", request.itemType);
        if (!Enum.IsDefined(typeof(Global.HelpCenterItems), request.itemType))
        {
            logger.LogWarning("Invalid HelpCenterItemType: {ItemType}", request.itemType);
            throw new BadHttpRequestException(
                string.Format(
                    localizationService.GetMessage("InvalidItemType"),
                    nameof(request.itemType),
                    request.itemType
                )
            );
        }

        // Fetch HelpCenter items
        logger.LogInformation("Handling GetHelpCenterItemQuery for itemType: {ItemType}", request.itemType);
        var terms = await helpCenterRepository.GetAllAsync(request.itemType);

        logger.LogInformation("Successfully retrieved {Count} HelpCenter items for itemType: {ItemType}", terms.Count, request.itemType);
        return terms;
    }
}