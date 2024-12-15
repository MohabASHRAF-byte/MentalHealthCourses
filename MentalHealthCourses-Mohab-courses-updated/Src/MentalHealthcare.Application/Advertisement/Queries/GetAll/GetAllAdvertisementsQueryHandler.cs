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
    ):IRequestHandler<GetAllAdvertisementsQuery, PageResult<AdvertisementDto>>
{
    public async Task<PageResult<AdvertisementDto>> Handle(GetAllAdvertisementsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetAllAdvertisementsQuery");
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        {
            logger.LogWarning("Unauthorized attempt to get all advertisements by user: {UserId}", currentUser?.Id);
            throw new ForBidenException("Don't have the permission to get all advertisements");
        }
        var ads = await advertisementRepository.GetAdvertisementsAsync(
            request.PageNumber, request.PageSize, request.IsActive
            );
        var adsDto = mapper.Map<IEnumerable<AdvertisementDto>>(ads.Item2);
        
        return new PageResult<AdvertisementDto>(adsDto, ads.Item1,request.PageSize, request.PageNumber);
    }
}