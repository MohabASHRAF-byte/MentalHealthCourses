using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities.Courses;

namespace MentalHealthcare.Domain.Entities;

public class CourseSection
{
    public int CourseSectionId { get; set; }
    [MaxLength(Global.CourseSectionNameMaxLength)]
    public string Name { get; set; }=string.Empty;
    public int Order { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; }
    public List<CourseLesson> Lessons { get; set; } = new List<CourseLesson>();
}