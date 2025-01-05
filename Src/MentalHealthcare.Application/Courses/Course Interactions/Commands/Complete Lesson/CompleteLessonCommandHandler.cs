using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Course_Interactions.Commands.Complete_Lesson;

public class CompleteLessonCommandHandler(
    ILogger<CompleteLessonCommandHandler> logger,
    ICourseInteractionsRepository courseInteractionsRepository,
    IUserContext userContext
) : IRequestHandler<CompleteLessonCommand>
{
    public async Task Handle(CompleteLessonCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting Handle method for CompleteLessonCommand with CourseId: {CourseId} and LessonId: {LessonId}", request.CourseId, request.LessonId);

        // Retrieve the current user
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.User))
        {
            logger.LogWarning(
                "Unauthorized access attempt to complete a lesson. User information: {UserDetails}",
                currentUser == null
                    ? "User is null"
                    : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}"
            );
            throw new ForBidenException("You do not have permission to complete this lesson.");
        }

        logger.LogInformation("User with SysUserId: {SysUserId} is attempting to complete LessonId: {LessonId} in CourseId: {CourseId}",
            currentUser.SysUserId, request.LessonId, request.CourseId);

        try
        {
            await courseInteractionsRepository.CompleteLessonAsync(
                request.CourseId,
                request.LessonId,
                currentUser.SysUserId!.Value
            );
            logger.LogInformation("Successfully completed LessonId: {LessonId} for UserId: {UserId} in CourseId: {CourseId}",
                request.LessonId, currentUser.SysUserId, request.CourseId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to complete LessonId: {LessonId} for UserId: {UserId} in CourseId: {CourseId}",
                request.LessonId, currentUser.SysUserId, request.CourseId);
            throw;
        }
    }
}