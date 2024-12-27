using MentalHealthcare.Domain.Dtos.User;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Dtos.course;

public class UserReviewDto
{
    public int UserReviewId { get; set; }
    public string Content { get; set; } = "";
    public float Rating { get; set; }
    public int Days { get; set; }
    public int Months { get; set; }
    public int Years { get; set; }

    public int SystemUserId { get; set; }
    public SystemUserDto user { get; set; } = null!;
}