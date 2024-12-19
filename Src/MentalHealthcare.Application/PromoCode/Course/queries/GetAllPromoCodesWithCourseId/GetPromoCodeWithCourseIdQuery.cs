using System.Text.Json.Serialization;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos.PromoCode;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Application.PromoCode.Course.queries.GetAllPromoCodesWithCourseId;

public class GetPromoCodeWithCourseIdQuery : IRequest<PageResult<CoursePromoCodeDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SearchText { get; set; } = "";
    public int IsActive { set; get; } = 2;
    [System.Text.Json.Serialization.JsonIgnore]
    public int CourseId { get; set; }
}