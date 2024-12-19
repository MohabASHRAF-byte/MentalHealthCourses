using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos.PromoCode;

namespace MentalHealthcare.Application.PromoCode.General.Queries.GetAllGeneralPromoCode;

public class GetAllGeneralPromoCodeQuery : IRequest<PageResult<GeneralPromoCodeDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SearchText { get; set; } = "";
    public int IsActive { set; get; } = 2;
}