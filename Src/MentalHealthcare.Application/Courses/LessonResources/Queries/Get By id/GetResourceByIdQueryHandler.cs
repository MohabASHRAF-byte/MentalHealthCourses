using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Courses.LessonResources.Queries.Get_By_id;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Exceptions;
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
        userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);

        // Fetch the resource
        logger.LogInformation("Fetching resource with ID: {ResourceId}", request.ResourceId);
        var resource = await courseResourcesRepository.GetCourseLessonResourceByIdAsync(request.ResourceId);

        if (resource == null)
        {
            logger.LogWarning("Resource with ID {ResourceId} not found.", request.ResourceId);
            throw new ResourceNotFound(
                "Resource",
                "مورد",
                request.ResourceId.ToString());
        }

        logger.LogInformation("Successfully fetched resource with ID: {ResourceId}", request.ResourceId);

        // Map resource to DTO
        var resourceDto = mapper.Map<CourseResourceDto>(resource);
        logger.LogInformation("Successfully mapped resource to DTO.");

        return resourceDto;
    }
}