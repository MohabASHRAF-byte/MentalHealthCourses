namespace MentalHealthcare.Domain.Entities;

public class UserReview
{
    public int UserReviewId { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
    
    public int courseId { get; set; }
    public Course course { get; set; }
    
    
}