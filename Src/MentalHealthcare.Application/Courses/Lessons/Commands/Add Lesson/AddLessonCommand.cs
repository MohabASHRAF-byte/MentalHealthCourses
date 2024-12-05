using MediatR;

namespace MentalHealthcare.Application.Courses.Lessons.Commands.Add_Lesson;

public class AddLessonCommand : IRequest<int>
{
    [System.Text.Json.Serialization.JsonIgnore]

    public int CourseId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]

    public int SectionId { get; set; }

    public string LessonName { get; set; } = string.Empty;
}