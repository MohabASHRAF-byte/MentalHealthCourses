using System.Text.Json.Serialization;
using MediatR;
using MentalHealthcare.Domain.Dtos.course;

namespace MentalHealthcare.Application.Courses.LessonResources.Queries.Get_By_id;

public class GetResourceByIdQuery : IRequest<CourseResourceDto>
{
    [JsonIgnore] public int CourseId { get; set; }
    [JsonIgnore] public int SectionId { get; set; }
    [JsonIgnore] public int LessonId { get; set; }
    [JsonIgnore] public int ResourceId { get; set; }
}