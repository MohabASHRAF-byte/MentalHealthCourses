using MediatR;

namespace MentalHealthcare.Application.Courses.Course.Commands.DeleteThumbnail;

public class DeleteCourseThumbnailCommand:IRequest
{
    public int CourseId { get; set; }
}