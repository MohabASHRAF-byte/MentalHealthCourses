using MediatR;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Application.Courses.Queries.GetById;

public class GetCourseByIdQuery:IRequest<CourseDto>
{
    public int Id { get; set; }
}