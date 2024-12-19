using MediatR;
using MentalHealthcare.Domain.Dtos.PromoCode;

namespace MentalHealthcare.Application.PromoCode.General.Queries.GetGeneralPromoCodeQuery;

public class GetGeneralPromoCodeQuery:IRequest<GeneralPromoCodeDto>
{
    public int PromoCodeId { get; set; }
}