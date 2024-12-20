using System.ComponentModel.DataAnnotations;
using MediatR;
using MentalHealthcare.Application.Videos.Commands.CreateVideo;

namespace MentalHealthcare.Application.Courses.Lessons.Commands.CreateVideo;

public class CreateVideoCommand : IRequest<CreateVideoCommandResponse>
{
    public string Title { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public int CourseId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]

    public int CourseSectionId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]

    public int LessonId { get; set; }

    [MaxLength(500)] public string? Description { set; get; }
}