using MediatR;

namespace MentalHealthcare.Application.Courses.Commands.DeleteThumbnail;

public class DeleteCourseThumbnailCommand:IRequest
{
    public int CourseId { get; set; }
}