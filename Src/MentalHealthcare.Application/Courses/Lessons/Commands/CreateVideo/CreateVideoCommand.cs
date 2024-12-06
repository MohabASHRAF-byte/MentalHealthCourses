using System.ComponentModel.DataAnnotations;
using MediatR;
using MentalHealthcare.Application.Videos.Commands.CreateVideo;

namespace MentalHealthcare.Application.Courses.Materials.Commands.CreateVideo;

public class CreateVideoCommand : IRequest<CreateVideoCommandResponse>
{
    public string VideoName { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public int CourseId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]

    public int SectionId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]

    public int LessonId { get; set; }

    [MaxLength(500)] public string? Description { set; get; }
}