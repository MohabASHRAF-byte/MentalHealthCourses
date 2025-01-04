using AutoMapper;
using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Advertisement.Queries.GetById;

public class GetAdvertisementByIdQueryHandler(
    ILogger<GetAdvertisementByIdQueryHandler> logger,
    IAdvertisementRepository advertisementRepository,
    IMapper mapper,
    IUserContext userContext
) : IRequestHandler<GetAdvertisementByIdQuery, AdvertisementDto>
{
    public async Task<AdvertisementDto> Handle(GetAdvertisementByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetAdvertisementByIdQuery for Advertisement ID: {AdId}", request.AdvertisementId);

        // Authorize user
        logger.LogInformation("Authorizing user for retrieving advertisement by ID.");
        var currentUser = userContext.EnsureAuthorizedUser([ UserRoles.Admin ], logger);
        logger.LogInformation("User {UserId} authorized to retrieve advertisement.", currentUser.Id);

        // Fetch advertisement
        logger.LogInformation("Fetching advertisement with ID: {AdId}", request.AdvertisementId);
        var ad = await advertisementRepository.GetAdvertisementByIdAsync(request.AdvertisementId);
        if (ad == null)
        {
            logger.LogWarning("Advertisement with ID: {AdId} not found.", request.AdvertisementId);
            throw new KeyNotFoundException($"Advertisement with ID {request.AdvertisementId} not found.");
        }

        // Map to DTO
        logger.LogInformation("Mapping advertisement to DTO for Advertisement ID: {AdId}.", request.AdvertisementId);
        var adDto = mapper.Map<AdvertisementDto>(ad);

        logger.LogInformation("Successfully retrieved and mapped advertisement with ID: {AdId}.", request.AdvertisementId);
        return adDto;
    }
}
