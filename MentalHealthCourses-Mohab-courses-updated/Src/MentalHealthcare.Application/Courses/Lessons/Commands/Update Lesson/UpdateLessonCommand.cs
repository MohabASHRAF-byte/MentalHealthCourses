using MediatR;

namespace MentalHealthcare.Application.Courses.Lessons.Commands.Update_Lesson;

public class UpdateLessonCommand : IRequest<int>
{
    [System.Text.Json.Serialization.JsonIgnore]

    public int CourseId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]

    public int SectionId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]

    public int LessonId { get; set; }

    public string LessonName { get; set; } = string.Empty;
}