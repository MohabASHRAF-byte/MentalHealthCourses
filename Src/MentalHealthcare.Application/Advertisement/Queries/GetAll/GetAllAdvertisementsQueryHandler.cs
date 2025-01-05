using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Advertisement.Queries.GetAll;

public class GetAllAdvertisementsQueryHandler(
    ILogger<GetAllAdvertisementsQueryHandler> logger,
    IAdvertisementRepository advertisementRepository,
    IMapper mapper,
    IUserContext userContext
) : IRequestHandler<GetAllAdvertisementsQuery, PageResult<AdvertisementDto>>
{
    public async Task<PageResult<AdvertisementDto>> Handle(GetAllAdvertisementsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetAllAdvertisementsQuery with PageNumber: {PageNumber}, PageSize: {PageSize}, IsActive: {IsActive}", 
            request.PageNumber, request.PageSize, request.IsActive);

        // Authorize user
        logger.LogInformation("Authorizing user for retrieving all advertisements.");
        var currentUser = userContext.EnsureAuthorizedUser(new List<UserRoles> { UserRoles.Admin }, logger);
        logger.LogInformation("User {UserId} authorized to retrieve all advertisements.", currentUser.Id);

        // Fetch advertisements
        logger.LogInformation("Fetching advertisements from the repository.");
        var ads = await advertisementRepository.GetAdvertisementsAsync(
            request.PageNumber, request.PageSize, request.IsActive
        );

        logger.LogInformation("Advertisements fetched successfully. Total records: {TotalRecords}", ads.Item1);

        // Map to DTOs
        logger.LogInformation("Mapping advertisements to DTOs.");
        var adsDto = mapper.Map<IEnumerable<AdvertisementDto>>(ads.Item2);

        logger.LogInformation("Returning PageResult with {TotalPages} total pages and {TotalRecords} total records", 
            (int)Math.Ceiling((double)ads.Item1 / request.PageSize), ads.Item1);

        return new PageResult<AdvertisementDto>(adsDto, ads.Item1, request.PageSize, request.PageNumber);
    }
}
