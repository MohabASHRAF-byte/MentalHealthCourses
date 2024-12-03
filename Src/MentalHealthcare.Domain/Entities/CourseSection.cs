using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Domain.Entities;

public class CourseSection
{
    public int CourseSectionId { get; set; }
    [MaxLength(Global.CourseSectionNameMaxLength)]
    public string Name { get; set; }=string.Empty;
    public int Order { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; }
}