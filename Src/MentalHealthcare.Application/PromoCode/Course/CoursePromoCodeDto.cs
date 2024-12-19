namespace MentalHealthcare.Application.PromoCode.Course;

public class CoursePromoCodeDto
{
    public int CoursePromoCodeId { get; set; }

    public string Code { get; set; }
    public DateTime expiredate { get; set; }
    public int expiresInDays { get; set; }
    public float percentage { get; set; }
    public int CourseId { get; set; }
    public string CourseName { get; set; }
}