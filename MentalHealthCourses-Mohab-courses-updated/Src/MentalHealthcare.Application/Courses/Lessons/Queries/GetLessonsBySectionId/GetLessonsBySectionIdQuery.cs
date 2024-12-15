using MediatR;
using MentalHealthcare.Domain.Dtos.course;

namespace MentalHealthcare.Application.Courses.Lessons.Queries.GetLessonsBySectionId;

public class GetLessonsBySectionIdQuery:IRequest<List<CourseLessonViewDto>>
{
    [System.Text.Json.Serialization.JsonIgnore]
    public int CourseId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]

    public int CourseSectionId { get; set; }
}