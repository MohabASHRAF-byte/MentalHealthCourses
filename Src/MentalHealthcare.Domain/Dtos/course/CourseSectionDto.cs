using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Dtos.course;

public class CourseSectionDto
{
    public int CourseSectionId { get; set; }
    [MaxLength(Global.CourseSectionNameMaxLength)]
   
    [JsonIgnore]
    public int Order { get; set; }
    public List<CourseLessonDto> Lessons { get; set; } = new ();

}