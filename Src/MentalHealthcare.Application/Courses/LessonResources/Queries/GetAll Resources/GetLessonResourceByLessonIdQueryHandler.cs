using AutoMapper;
using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
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
    IMapper mapper,
    IUserContext userContext
) : IRequestHandler<GetLessonResourceByLessonIdQuery, List<CourseResourceDto>>
{
    public async Task<List<CourseResourceDto>> Handle(GetLessonResourceByLessonIdQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting GetLessonResourceByLessonIdQuery for Lesson ID: {LessonId}", request.LessonId);

        // Authenticate and validate admin permissions
        userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);

        // Fetch resources for the lesson
        var resources = await courseResourcesRepository.GetCourseLessonResourcesByCourseIdAsync(request.LessonId);

        // Map resources to DTO
        var dtos = mapper.Map<List<CourseResourceDto>>(resources);

        logger.LogInformation("Successfully fetched and mapped resources for Lesson ID: {LessonId}", request.LessonId);

        return dtos;
    }
}