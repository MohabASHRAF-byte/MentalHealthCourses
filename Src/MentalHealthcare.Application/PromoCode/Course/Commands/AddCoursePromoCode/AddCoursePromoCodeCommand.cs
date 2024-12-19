using MediatR;

namespace MentalHealthcare.Application.PromoCode.Course.Commands.AddCoursePromoCode;

public class AddCoursePromoCodeCommand:IRequest<int>
{
    public string Code { get; set; }

    public string ExpireDate { get; set; } 

    public float Percentage { get; set; }

    public int CourseId { get; set; }
}