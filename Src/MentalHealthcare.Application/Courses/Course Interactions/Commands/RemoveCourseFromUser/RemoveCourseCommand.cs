using MediatR;

namespace MentalHealthcare.Application.Courses.Course_Interactions.Commands.RemoveCourseFromUser;

public class RemoveCourseCommand : IRequest
{
    public int UserId { get; set; }
    public int CourseId { get; set; }
}