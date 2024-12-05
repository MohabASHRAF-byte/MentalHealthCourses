using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Advertisement.Queries.GetAll;

public class GetAllAdvertisementsQueryHandler(
    ILogger<GetAllAdvertisementsQueryHandler> logger,
    IAdvertisementRepository advertisementRepository,
    IMapper mapper
    ):IRequestHandler<GetAllAdvertisementsQuery, PageResult<AdvertisementDto>>
{
    public async Task<PageResult<AdvertisementDto>> Handle(GetAllAdvertisementsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetAllAdvertisementsQuery");
        var ads = await advertisementRepository.GetAdvertisementsAsync(
            request.PageNumber, request.PageSize, request.IsActive
            );
        var adsDto = mapper.Map<IEnumerable<AdvertisementDto>>(ads.Item2);
        
        return new PageResult<AdvertisementDto>(adsDto, ads.Item1,request.PageSize, request.PageNumber);
    }
}