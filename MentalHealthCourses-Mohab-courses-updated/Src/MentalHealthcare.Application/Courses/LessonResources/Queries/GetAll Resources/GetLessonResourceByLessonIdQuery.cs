using System.Text.Json.Serialization;
using MediatR;
using MentalHealthcare.Domain.Dtos.course;

namespace MentalHealthcare.Application.Courses.LessonResources.Queries.GetAll_Resources;

public class GetLessonResourceByLessonIdQuery:IRequest<List<CourseResourceDto>>
{
    [JsonIgnore] public int CourseId { get; set; }
    [JsonIgnore] public int SectionId { get; set; }
    [JsonIgnore] public int LessonId { get; set; }
}