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
    public async Task<List<CourseResourceDto>> Handle(GetLessonResourceByLessonIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting GetLessonResourceByLessonIdQuery for Lesson ID: {LessonId}", request.LessonId);

        // Authenticate and validate admin permissions
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        {
            var userDetails = currentUser == null
                ? "User is null"
                : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}";

            logger.LogWarning("Unauthorized access attempt to fetch lesson resources. User details: {UserDetails}", userDetails);
            throw new UnauthorizedAccessException("You do not have permission to access this resource.");
        }

        logger.LogInformation("Admin access granted for user {UserId}", currentUser.Id);

        // Fetch resources for the lesson
        var resources = await courseResourcesRepository.GetCourseLessonResourcesByCourseIdAsync(request.LessonId);

        // Map resources to DTO
        var dtos = mapper.Map<List<CourseResourceDto>>(resources);

        logger.LogInformation("Successfully fetched and mapped resources for Lesson ID: {LessonId}", request.LessonId);

        return dtos;
    }
}
