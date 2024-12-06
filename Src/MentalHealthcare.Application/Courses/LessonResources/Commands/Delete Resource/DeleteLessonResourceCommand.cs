using System.Text.Json.Serialization;
using MediatR;

namespace MentalHealthcare.Application.Courses.LessonResources.Commands.Delete_Resource;

public class DeleteLessonResourceCommand:IRequest
{
    [JsonIgnore]
    public int CourseId { get; set; }
    [JsonIgnore]
    public int SectionId { get; set; }
    [JsonIgnore]
    public int LessonId { get; set; }
    public int ResourceId { get; set; }

}