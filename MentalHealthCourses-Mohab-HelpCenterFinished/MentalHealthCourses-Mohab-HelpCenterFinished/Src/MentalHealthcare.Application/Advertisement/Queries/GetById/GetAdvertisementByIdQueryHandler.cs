using AutoMapper;
using MediatR;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Advertisement.Queries.GetById;

public class GetAdvertisementByIdQueryHandler(
    ILogger<GetAdvertisementByIdQueryHandler> logger,
    IAdvertisementRepository advertisementRepository,
    IMapper mapper
    ):IRequestHandler<GetAdvertisementByIdQuery, AdvertisementDto>
{
    public async Task<AdvertisementDto> Handle(GetAdvertisementByIdQuery request, CancellationToken cancellationToken)
    {
       logger.LogInformation($"GetAdvertisementByIdQueryHandler invoked.");
       logger.LogInformation($"GetAdvertisementByIdQueryHandler. Request: {request.AdvertisementId}");
       var ad = await advertisementRepository.GetAdvertisementByIdAsync(request.AdvertisementId);
       var adDto = mapper.Map<AdvertisementDto>(ad);
       return adDto;
    }
}