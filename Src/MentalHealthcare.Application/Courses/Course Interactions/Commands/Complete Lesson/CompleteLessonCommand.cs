using MediatR;

namespace MentalHealthcare.Application.Courses.Course_Interactions.Commands.Complete_Lesson;

public class CompleteLessonCommand : IRequest
{
    public int CourseId { get; set; }
    public int LessonId { get; set; }
}