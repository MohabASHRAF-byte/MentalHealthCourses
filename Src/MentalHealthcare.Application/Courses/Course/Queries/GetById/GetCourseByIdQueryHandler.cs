using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Courses.Commands.Create;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Queries.GetById;

public class GetCourseByIdQueryHandler(
    ILogger<CreateCourseCommandHandler> logger,
    IMapper mapper,
    ICourseRepository courseRepository
) : IRequestHandler<GetCourseByIdQuery, CourseDto>
{
    public async Task<CourseDto> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetMinimalCourseByIdAsync(request.Id);
        var courseDto = mapper.Map<CourseDto>(course);
        courseDto.Rating ??= 0;
        if (courseDto.ReviewsCount!= 0)
            courseDto.Rating /= courseDto.ReviewsCount;
        return courseDto;
    }
}