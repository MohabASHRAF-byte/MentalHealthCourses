using MentalHealthcare.Domain.Dtos.User;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Dtos.course;

public class UserReviewDto
{
    public int UserReviewId { get; set; }
    public string Content { get; set; } = "";
    public float Rating { get; set; }
    
    public long  Seconds { get; set; }

    public int SystemUserId { get; set; }
    public SystemUserDto user { get; set; } = null!;
}