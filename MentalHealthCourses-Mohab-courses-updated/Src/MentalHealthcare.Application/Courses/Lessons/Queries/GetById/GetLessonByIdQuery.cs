using MediatR;
using MentalHealthcare.Domain.Dtos.course;

namespace MentalHealthcare.Application.Courses.Lessons.Queries.GetById;

public class GetLessonByIdQuery:IRequest<CourseLessonDto>
{
    [System.Text.Json.Serialization.JsonIgnore]
    public int CourseId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]

    public int CourseSectionId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]

    public int LessonId { get; set; }
}