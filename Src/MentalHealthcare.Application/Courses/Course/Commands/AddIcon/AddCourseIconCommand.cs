using MediatR;
using Microsoft.AspNetCore.Http;

namespace MentalHealthcare.Application.Courses.Course.Commands.AddIcon;

public class AddCourseIconCommand:IRequest
{
    public int CourseId { get; set; }
    public IFormFile File { get; set; } = default!;
}