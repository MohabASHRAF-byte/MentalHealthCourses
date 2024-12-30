using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MediatR;
using MentalHealthcare.Domain.Constants;
using Microsoft.AspNetCore.Http;

namespace MentalHealthcare.Application.Courses.Course.Commands.Create;

public class CreateCourseCommand : IRequest<CreateCourseCommandResponse>
{
    public string Name { set; get; } = default!;
    public decimal Price { get; set; }
    public string Description { get; set; } = String.Empty;
    public int InstructorId { get; set; } = default!;
    public List<int> CategoryId { get; set; } = [];
}