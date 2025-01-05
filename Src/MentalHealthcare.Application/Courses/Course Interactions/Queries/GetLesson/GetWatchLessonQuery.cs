using MediatR;
using MentalHealthcare.Domain.Dtos.course;

namespace MentalHealthcare.Application.Courses.Course_Interactions.Queries.GetLesson;

public class GetWatchLessonQuery : IRequest<CourseLessonDto>
{
    public int CourseId { get; set; }
    public int LessonId { get; set; }
}