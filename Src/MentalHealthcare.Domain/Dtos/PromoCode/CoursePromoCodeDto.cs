namespace MentalHealthcare.Domain.Dtos.PromoCode;

public class CoursePromoCodeDto
{
    public int CoursePromoCodeId { get; set; }

    public string Code { get; set; }
    public DateTime expiredate { get; set; }
    public int expiresInDays { get; set; }
    public long SecondsTillExpire { get; set; }
    public float percentage { get; set; }
    public int CourseId { get; set; }
    public string CourseName { get; set; }
    public bool IsActive { get; set; }

}