using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Courses.LessonResources.Queries.Get_By_id;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.LessonResources.Queries.Get_By_Id;

public class GetResourceByIdQueryHandler(
    ILogger<GetResourceByIdQueryHandler> logger,
    ICourseResourcesRepository courseResourcesRepository,
    IMapper mapper,
    IUserContext userContext
) : IRequestHandler<GetResourceByIdQuery, CourseResourceDto>
{
    public async Task<CourseResourceDto> Handle(GetResourceByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting GetResourceByIdQuery for Resource ID: {ResourceId}", request.ResourceId);

        // Authenticate and validate admin permissions
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        {
            var userDetails = currentUser == null
                ? "User is null"
                : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}";

            logger.LogWarning("Unauthorized access attempt to fetch resource. User details: {UserDetails}", userDetails);
            throw new UnauthorizedAccessException("You do not have permission to access this resource.");
        }

        logger.LogInformation("Admin access granted for user {UserId}", currentUser.Id);

        // Fetch the resource
        logger.LogInformation("Fetching resource with ID: {ResourceId}", request.ResourceId);
        var resource = await courseResourcesRepository.GetCourseLessonResourceByIdAsync(request.ResourceId);

        if (resource == null)
        {
            logger.LogWarning("Resource with ID {ResourceId} not found.", request.ResourceId);
            throw new KeyNotFoundException($"Resource with ID {request.ResourceId} not found.");
        }

        logger.LogInformation("Successfully fetched resource with ID: {ResourceId}", request.ResourceId);

        // Map resource to DTO
        var resourceDto = mapper.Map<CourseResourceDto>(resource);
        logger.LogInformation("Successfully mapped resource to DTO.");

        return resourceDto;
    }
}
