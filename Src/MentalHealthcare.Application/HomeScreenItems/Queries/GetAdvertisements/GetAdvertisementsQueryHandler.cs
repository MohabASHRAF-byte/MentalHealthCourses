using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Advertisement.Queries.GetAll;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.HomeScreenItems.Queries.GetAdvertisements
{
    public class GetAdvertisementsQueryHandler(
    ILogger<GetAdvertisementsQueryHandler> logger,
    IHomeScreenRepository homeScreenRepository,
    IMapper mapper,
    IUserContext userContext
) : IRequestHandler<GetAdvertisementsQuery, PageResult<AdvertisementDto>>
    {
        public async Task<PageResult<AdvertisementDto>> Handle(GetAdvertisementsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetAllAdvertisementsQuery with PageNumber: {PageNumber}, PageSize: {PageSize}, IsActive: {IsActive}",
                       request.PageNumber, request.PageSize, request.IsActive);

            // Authorize user
            logger.LogInformation("Authorizing user for retrieving all advertisements.");
            var currentUser = userContext.EnsureAuthorizedUser(new List<UserRoles> { UserRoles.User }, logger);
            logger.LogInformation("User {UserId} authorized to retrieve all advertisements.", currentUser.Id);

            // Fetch advertisements
            logger.LogInformation("Fetching advertisements from the repository.");
            var ads = await homeScreenRepository.GetAdvertisementsAsync(
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
}
