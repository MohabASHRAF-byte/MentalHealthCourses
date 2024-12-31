using MediatR;

namespace MentalHealthcare.Application.Courses.Course.Commands.DeleteIconCommand;

public class DeleteCourseIconCommand:IRequest
{
    public int CourseId { get; set; }

}