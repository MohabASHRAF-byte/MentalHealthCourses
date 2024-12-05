using MediatR;
using MentalHealthcare.Domain.Dtos;

namespace MentalHealthcare.Application.Courses.Queries.GetById;

public class GetCourseByIdQuery:IRequest<CourseDto>
{
    public int Id { get; set; }
}