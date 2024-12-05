using System.ComponentModel.DataAnnotations;
using MediatR;
using MentalHealthcare.Domain.Constants;
using Microsoft.AspNetCore.Http;

namespace MentalHealthcare.Application.Courses.Commands.Create;

public class CreateCourseCommand : IRequest<CreateCourseCommandResponse>
{
    [MaxLength(Global.TitleMaxLength)] public string Name { set; get; } = default!;
    public IFormFile? Thumbnail { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }=String.Empty;
    public bool IsFree { get; set; } = false;
    public int InstructorId { get; set; } = default!;
}