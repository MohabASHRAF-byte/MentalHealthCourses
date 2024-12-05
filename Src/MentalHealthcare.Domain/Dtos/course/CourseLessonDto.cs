using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Dtos.course;

public class CourseLessonDto
{
    public int CourseLessonId { get; set; }
    [MaxLength(500)]
    public string LessonName { get; set; } = string.Empty;
    public int Order { get; set; }

    public List<CourseMaterielDto> CourseMateriels { get; set; } = new();
    
}