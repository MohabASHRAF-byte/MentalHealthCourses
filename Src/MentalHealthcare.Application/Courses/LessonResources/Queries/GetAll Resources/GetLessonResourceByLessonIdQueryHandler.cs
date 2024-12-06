using AutoMapper;
using MediatR;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.LessonResources.Queries.GetAll_Resources;

public class GetLessonResourceByLessonIdQueryHandler(
    ILogger<GetLessonResourceByLessonIdQueryHandler> logger,
    ICourseResourcesRepository courseResourcesRepository,
    ICourseRepository courseRepository,
    IConfiguration configuration,
    IMapper mapper
) : IRequestHandler<GetLessonResourceByLessonIdQuery, List<CourseResourceDto>>
{
    public async Task<List<CourseResourceDto>> Handle(GetLessonResourceByLessonIdQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handle request for GetLessonResourceByLessonIdQuery");
        var resources = await courseResourcesRepository.GetCourseLessonResourcesByCourseIdAsync(request.LessonId);
        var dtos = mapper.Map<List<CourseResourceDto>>(resources);
        return dtos;
    }
}