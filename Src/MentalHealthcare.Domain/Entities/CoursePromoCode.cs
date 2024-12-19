namespace MentalHealthcare.Domain.Entities;

public class CoursePromoCode
{
    public int CoursePromoCodeId { get; set; }

    public string Code { get; set; }

    public DateTime expiredate { get; set; }

    public float percentage { get; set; }

    //Relations
    public int CourseId { get; set; }
    public Course Course { get; set; }
}