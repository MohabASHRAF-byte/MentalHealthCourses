using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Domain.Dtos.course;

public class CourseSectionViewDto
{
    public int CourseSectionId { get; set; }
    [MaxLength(Global.CourseSectionNameMaxLength)]
    public string Name { get; set; }=string.Empty;
    public int Order { get; set; }
    public int LessonsCount { get; set; }

}