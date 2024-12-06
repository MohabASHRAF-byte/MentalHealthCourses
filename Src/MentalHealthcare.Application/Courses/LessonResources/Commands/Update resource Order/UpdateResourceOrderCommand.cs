using System.Text.Json.Serialization;
using MediatR;

namespace MentalHealthcare.Application.Courses.LessonResources.Commands.Update_resource_Order;

public class UpdateResourceOrderCommand:IRequest<int>
{
    [JsonIgnore]
    public int CourseId { get; set; }
    [JsonIgnore]
    public int SectionId { get; set; }
    [JsonIgnore]
    public int LessonId { get; set; }  
    public List<ResourceOrderDto> Orders { get; set; } = new();
}

public class ResourceOrderDto
{
    public int ResourceId { get; set; }
    public int Order { get; set; }
}