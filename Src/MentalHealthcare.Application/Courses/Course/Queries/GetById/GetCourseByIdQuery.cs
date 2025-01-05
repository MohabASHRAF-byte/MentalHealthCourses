using MediatR;
using MentalHealthcare.Domain.Dtos.course;

namespace MentalHealthcare.Application.Courses.Course.Queries.GetById;

public class GetCourseByIdQuery:IRequest<CourseDto>
{
    public int Id { get; set; }
}