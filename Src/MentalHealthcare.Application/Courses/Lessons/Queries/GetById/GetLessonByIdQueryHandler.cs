using AutoMapper;
using MediatR;
using MentalHealthcare.Domain.Repositories.Course;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Lessons.Queries.GetById;

public class GetLessonByIdQueryHandler(
    ILogger<GetLessonByIdQueryHandler> logger,
    ICourseLessonRepository lessonRepository,
    IMapper mapper,
    IUserContext userContext
) : IRequestHandler<GetLessonByIdQuery, CourseLessonDto>
{
    public async Task<CourseLessonDto> Handle(GetLessonByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching lesson with ID {LessonId}", request.LessonId);

        // Authenticate and validate admin permissions
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        {
            var userDetails = currentUser == null
                ? "User is null"
                : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}";

            logger.LogWarning("Unauthorized access attempt to fetch lesson. User details: {UserDetails}", userDetails);
            throw new ForBidenException("You do not have permission to access this resource.");
        }

        logger.LogInformation("Admin access granted for user {UserId}", currentUser.Id);

        // Retrieve the lesson by ID
        var lesson = await lessonRepository.GetCourseFullLessonByIdAsync(request.LessonId);

        if (lesson == null)
        {
            logger.LogWarning("Lesson with ID {LessonId} not found.", request.LessonId);
            throw new ResourceNotFound("Lesson",request.LessonId.ToString());
        }

        logger.LogInformation("Successfully fetched lesson with ID {LessonId}. Mapping to DTO.", request.LessonId);

        // Map the lesson entity to a DTO
        var lessonDto = mapper.Map<CourseLessonDto>(lesson);

        logger.LogInformation("Successfully mapped lesson ID {LessonId} to DTO.", request.LessonId);

        return lessonDto;
    }
}
