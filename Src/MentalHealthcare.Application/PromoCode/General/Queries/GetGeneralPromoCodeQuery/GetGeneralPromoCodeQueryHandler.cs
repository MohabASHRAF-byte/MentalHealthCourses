using AutoMapper;
using MediatR;
using MentalHealthcare.Domain.Dtos.PromoCode;
using MentalHealthcare.Domain.Repositories.PromoCode;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.PromoCode.General.Queries.GetGeneralPromoCodeQuery;

public class GetGeneralPromoCodeQueryHandler(
    ILogger<GetGeneralPromoCodeQueryHandler> logger,
    IMapper mapper,
    IGeneralPromoCodeRepository generalPromoCodeRepository
) : IRequestHandler<GetGeneralPromoCodeQuery, GeneralPromoCodeDto>
{
    public async Task<GeneralPromoCodeDto> Handle(GetGeneralPromoCodeQuery request, CancellationToken cancellationToken)
    {
        //TODO: add auth 
        logger.LogInformation("Handling GetgeneralPromoCodeQuery");
        var generalPromoCode = await generalPromoCodeRepository
            .GetGeneralPromoCodeByIdAsync(request.PromoCodeId);
        var generalPromoCodeDto = mapper.Map<GeneralPromoCodeDto>(generalPromoCode);
        return generalPromoCodeDto;
    }
}