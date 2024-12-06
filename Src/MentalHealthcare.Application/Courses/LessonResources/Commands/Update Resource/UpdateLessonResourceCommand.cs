using System.Text.Json.Serialization;
using MediatR;

namespace MentalHealthcare.Application.Courses.LessonResources.Commands.Update_Resource;

public class UpdateLessonResourceCommand:IRequest<int>
{
    [JsonIgnore]
    public int CourseId { get; set; }
    [JsonIgnore]
    public int SectionId { get; set; }
    [JsonIgnore]
    public int LessonId { get; set; }
    public int ResourceId { get; set; }
    public string ResourceName { get; set; }
}