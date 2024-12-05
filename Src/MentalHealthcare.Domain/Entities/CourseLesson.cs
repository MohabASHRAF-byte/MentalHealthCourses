using System.ComponentModel.DataAnnotations;

namespace MentalHealthcare.Domain.Entities;

public class CourseLesson
{
    public int CourseLessonId { get; set; }
    [MaxLength(500)]
    public string LessonName { get; set; } = string.Empty;
    public int Order { get; set; }

    public List<CourseMateriel> CourseMateriels { get; set; } = new();

    //
    public int CourseSectionId { get; set; }

    public CourseSection CourseSection { get; set; }

    //
    public int CourseId { get; set; }
    public Course Course { get; set; }
}