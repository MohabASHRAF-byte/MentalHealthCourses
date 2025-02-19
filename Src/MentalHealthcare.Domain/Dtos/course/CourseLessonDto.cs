using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Dtos.course;

public class CourseLessonDto
{
    public int CourseLessonId { get; set; }
    [MaxLength(500)] public string LessonName { get; set; } = string.Empty;
    // public int Order { get; set; }
    public int OrderOnCourse { get; set; }


    public List<CourseResourceDto> CourseLessonResources { get; set; } = new();
    public int views { get; set; } = 0;
    public string Url { get; set; } = string.Empty;
    public int LessonLengthInSeconds { get; set; } = 0;
    
    public ContentType ContentType { set; get; }
}