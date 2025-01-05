using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Course_Interactions.Queries.GetLesson;

public class GetWatchLessonQueryHandler(
    ILogger<GetWatchLessonQueryHandler> logger,
    ICourseInteractionsRepository courseInteractionsRepository,
    IUserContext userContext
) : IRequestHandler<GetWatchLessonQuery, CourseLessonDto>
{
    public async Task<CourseLessonDto> Handle(GetWatchLessonQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting Handle method for GetWatchLessonQuery with LessonId: {LessonId}", request.LessonId);

        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.User))
        {
            logger.LogWarning(
                "Unauthorized access attempt to fetch lesson. User information: {UserDetails}",
                currentUser == null
                    ? "User is null"
                    : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}"
            );
            throw new ForBidenException("You do not have permission to fetch this lesson.");
        }

        logger.LogInformation("User with SysUserId: {SysUserId} is attempting to fetch LessonId: {LessonId}",
            currentUser.SysUserId, request.LessonId);

        try
        {
            var courseLesson = await courseInteractionsRepository.GetLessonAsync(request.LessonId);

            logger.LogInformation("Successfully fetched LessonId: {LessonId} for UserId: {UserId}",
                request.LessonId, currentUser.SysUserId);

            return courseLesson;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch LessonId: {LessonId} for UserId: {UserId}",
                request.LessonId, currentUser.SysUserId);
            throw;
        }
    }
}
