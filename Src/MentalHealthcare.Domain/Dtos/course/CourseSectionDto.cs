using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Dtos.course;

public class CourseSectionDto
{
    public int CourseSectionId { get; set; }
    [MaxLength(Global.CourseSectionNameMaxLength)]
    public string Name { get; set; }=string.Empty;
    public int Order { get; set; }
    public List<CourseLessonDto> Lessons { get; set; } = new ();

}