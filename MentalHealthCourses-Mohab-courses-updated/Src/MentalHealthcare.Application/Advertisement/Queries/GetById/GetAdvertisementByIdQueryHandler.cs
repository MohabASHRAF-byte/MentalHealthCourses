using AutoMapper;
using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Advertisement.Queries.GetById;

public class GetAdvertisementByIdQueryHandler(
    ILogger<GetAdvertisementByIdQueryHandler> logger,
    IAdvertisementRepository advertisementRepository,
    IMapper mapper,
    IUserContext userContext
    ):IRequestHandler<GetAdvertisementByIdQuery, AdvertisementDto>
{
    public async Task<AdvertisementDto> Handle(GetAdvertisementByIdQuery request, CancellationToken cancellationToken)
    {
       logger.LogInformation($"GetAdvertisementByIdQueryHandler invoked.");
       logger.LogInformation($"GetAdvertisementByIdQueryHandler. Request: {request.AdvertisementId}");
       var currentUser = userContext.GetCurrentUser();
       if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
       {
           logger.LogWarning("Unauthorized attempt to get ad by user: {UserId}", currentUser?.Id);
           throw new ForBidenException("Don't have the permission to get advertisement");
       }
       var ad = await advertisementRepository.GetAdvertisementByIdAsync(request.AdvertisementId);
       var adDto = mapper.Map<AdvertisementDto>(ad);
       return adDto;
    }
}