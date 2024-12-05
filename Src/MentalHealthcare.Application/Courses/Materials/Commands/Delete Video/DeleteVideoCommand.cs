using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MentalHealthcare.Application.Courses.Materials.Commands.Delete_Video;

public class DeleteVideoCommand:IRequest
{
    [System.Text.Json.Serialization.JsonIgnore]
    public int CourseId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]

    public int SectionId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]

    public int LessonId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]

    public int MaterialId { get; set; }

}