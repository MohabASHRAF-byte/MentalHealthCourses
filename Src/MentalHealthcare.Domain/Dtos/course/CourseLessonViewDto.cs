using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Dtos.course;

public class CourseLessonViewDto
{
    public int CourseLessonId { get; set; }
    [MaxLength(500)]
    public string LessonName { get; set; } = string.Empty;
    public int Order { get; set; }
    public int LessonResourcesCount { get; set; }
}