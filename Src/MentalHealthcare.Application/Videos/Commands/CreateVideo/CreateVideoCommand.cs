using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MentalHealthcare.Application.Videos.Commands.CreateVideo;

public class CreateVideoCommand:IRequest<CreateVideoCommandResponse>
{
    public string VideoName { get; set; }
    public int CourseId { get; set; }
    [MaxLength(500)]
    public string? Description { set; get; } 
}