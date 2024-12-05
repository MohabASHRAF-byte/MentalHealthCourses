using MediatR;

namespace MentalHealthcare.Application.Courses.Materials.Commands.ConfirmUpload;

public class ConfirmUploadCommand : IRequest
{
    [System.Text.Json.Serialization.JsonIgnore]
    public int CourseId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]

    public int SectionId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]

    public int LessonId { get; set; }

    public string videoId { get; set; }

    public bool Confirmed { get; set; }
}