using MediatR;

namespace MentalHealthcare.Application.Courses.Course_Interactions.Commands.Enroll_Course;

public class EnrollCourseCommand : IRequest
{
    public int CourseId { get; set; }
}