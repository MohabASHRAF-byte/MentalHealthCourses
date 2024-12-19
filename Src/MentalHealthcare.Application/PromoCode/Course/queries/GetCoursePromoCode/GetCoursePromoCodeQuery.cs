using MediatR;
using MentalHealthcare.Domain.Dtos.PromoCode;

namespace MentalHealthcare.Application.PromoCode.Course.queries.GetCoursePromoCode;

public class GetCoursePromoCodeQuery : IRequest<CoursePromoCodeDto>
{
    public int CoursePromoCodeId { get; set; }
}