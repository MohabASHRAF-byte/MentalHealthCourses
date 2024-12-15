using System.Text.Json.Serialization;
using MediatR;

namespace MentalHealthcare.Application.Courses.Lessons.Commands.Update_order;

public class UpdateLessonsOrderCommand:IRequest
{
    [System.Text.Json.Serialization.JsonIgnore]
    public int CourseId { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]

    public int SectionId { get; set; }
    public List<LessonOrderDto> Orders { get; set; } = new ();
    
}
public class LessonOrderDto
{
    public int LessonId { get; set; }
    public int Order { get; set; }
}