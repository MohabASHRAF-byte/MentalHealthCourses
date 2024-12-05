using MediatR;

namespace MentalHealthcare.Application.Courses.Lessons.Commands.RemoveLesson;

public class RemoveLessonCommand:IRequest
{
    [System.Text.Json.Serialization.JsonIgnore]

    public int CourseId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]

    public int SectionId { get; set; }
    
    public int LessonId { get; set; }
}