using MediatR;
using Microsoft.AspNetCore.Http;

namespace MentalHealthcare.Application.Courses.Course.Commands.AddThumbnail;

public class AddCourseThumbnailCommand:IRequest<string>
{
    public int CourseId { get; set; }
    public IFormFile File { get; set; } = default!;
    
}