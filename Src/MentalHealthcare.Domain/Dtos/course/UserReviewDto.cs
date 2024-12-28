using MentalHealthcare.Domain.Dtos.User;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Dtos.course;

public class UserReviewDto
{
    public int UserReviewId { get; set; }
    public string Content { get; set; } = "";
    public bool IsFullContent { get; set; } = false;
    public bool IsEdited { get; set; } = false;
    public float Rating { get; set; }
    
    public long SecondsSinceCreated { get; set; }
    public long SecondsSinceLastEdited { get; set; }
    public int courseId { get; set; }

    public int SystemUserId { get; set; }
    public SystemUserDto user { get; set; } = null!;
}