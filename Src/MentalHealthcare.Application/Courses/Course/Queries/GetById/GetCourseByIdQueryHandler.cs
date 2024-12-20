using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Courses.Commands.Create;
using MentalHealthcare.Application.Courses.Queries.GetById;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Course.Queries.GetById;

public class GetCourseByIdQueryHandler(
    ILogger<CreateCourseCommandHandler> logger,
    IMapper mapper,
    ICourseRepository courseRepository
) : IRequestHandler<GetCourseByIdQuery, CourseDto>
{
    public async Task<CourseDto> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetFullCourseByIdAsync(request.Id);
        var courseDto = mapper.Map<CourseDto>(course);
        courseDto.Rating ??= 0;
        if (courseDto.ReviewsCount!= 0)
            courseDto.Rating /= courseDto.ReviewsCount;
        return courseDto;
    }
}