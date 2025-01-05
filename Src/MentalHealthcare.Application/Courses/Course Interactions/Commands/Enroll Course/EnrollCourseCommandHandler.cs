using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Course_Interactions.Commands.Enroll_Course;

public class EnrollCourseCommandHandler(
    ILogger<EnrollCourseCommandHandler> logger,
    ICourseInteractionsRepository courseInteractionsRepository,
    IUserContext userContext
) : IRequestHandler<EnrollCourseCommand>
{
    public async Task Handle(EnrollCourseCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting Handle method for EnrollCourseCommand with CourseId: {CourseId}", request.CourseId);

        // Retrieve the current user
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.User))
        {
            logger.LogWarning(
                "Unauthorized access attempt to enroll in a course. User information: {UserDetails}",
                currentUser == null ? "User is null" : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}"
            );
            throw new ForBidenException("You do not have permission to enroll in this course.");
        }

        logger.LogInformation("User with SysUserId: {SysUserId} is attempting to enroll in CourseId: {CourseId}",
            currentUser.SysUserId, request.CourseId);

        try
        {
            await courseInteractionsRepository.Enroll(request.CourseId, currentUser.SysUserId!.Value);
            logger.LogInformation("Successfully enrolled user with SysUserId: {SysUserId} into CourseId: {CourseId}",
                currentUser.SysUserId, request.CourseId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to enroll user with SysUserId: {SysUserId} into CourseId: {CourseId}",
                currentUser.SysUserId, request.CourseId);
            throw;
        }
    }
}
