using System.Text.Json.Serialization;
using MediatR;

namespace MentalHealthcare.Application.PromoCode.Course.Commands.UpdateCoursePromoCode;

public class UpdateCoursePromoCodeCommand : IRequest<int>
{
    [JsonIgnore]
    public int CoursePromoCodeId { get; set; }
    
    public string? ExpireDate { get; set; }

    public double? Percentage { get; set; }
    public bool? IsActive { get; set; }
}