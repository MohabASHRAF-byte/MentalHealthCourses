using MediatR;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Application.PromoCode.Course.Commands.DeleteCoursePromoCode;

public class DeleteCoursePromoCodeCommand:IRequest
{
    [System.Text.Json.Serialization.JsonIgnore]
    public int CoursePromoCodeId { get; set; }
}