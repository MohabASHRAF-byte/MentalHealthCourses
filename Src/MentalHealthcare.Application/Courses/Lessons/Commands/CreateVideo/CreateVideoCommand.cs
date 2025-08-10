using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MentalHealthcare.Application.Courses.Lessons.Commands.CreateVideo;

public class CreateVideoCommand : IRequest<CreateVideoCommandResponse>
{
    [MaxLength(500, ErrorMessage = "??? ??? ?????? ????? ??? ?????.")]
    public string Title { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public int CourseId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]

    public int CourseSectionId { get; set; }


    public int LengthWithSeconds { set; get; }
}