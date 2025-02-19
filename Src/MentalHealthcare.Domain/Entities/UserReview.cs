using MentalHealthcare.Domain.Entities.Courses;

namespace MentalHealthcare.Domain.Entities;

public class UserReview
{
    public int UserReviewId { get; set; }
    public string Content { get; set; } = "";
    public float Rating { get; set; }
    public DateTime ReviewDate { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;
    public bool IsEdited { get; set; } = false;

    public int SystemUserId { get; set; }
    public SystemUser User { get; set; }

    public int courseId { get; set; }
    public Course course { get; set; }
}