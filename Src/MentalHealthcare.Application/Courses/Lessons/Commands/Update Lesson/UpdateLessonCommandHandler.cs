using AutoMapper;
using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Lessons.Commands.Update_Lesson;

/// <summary>
/// Command handler for updating a course lesson.
/// </summary>
public class UpdateLessonCommandHandler(
    ILogger<UpdateLessonCommandHandler> logger,
    ICourseRepository courseRepository,
    ICourseLessonRepository courseLessonRepository,
    IUserContext userContext
) : IRequestHandler<UpdateLessonCommand, int>
{
    public async Task<int> Handle(UpdateLessonCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting UpdateLessonCommandHandler for LessonId: {LessonId}, LessonName: {LessonName}",
            request.LessonId, request.LessonName);

        // Authenticate and validate admin permissions
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        {
            var userDetails = currentUser == null
                ? "User is null"
                : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}";

            logger.LogWarning("Unauthorized access attempt to update lesson data. User details: {UserDetails}", userDetails);
            throw new ForBidenException("You do not have permission to update this lesson.");
        }

        logger.LogInformation("Validating if the lesson exists with LessonId: {LessonId}", request.LessonId);

        // Perform the update.
        await courseLessonRepository.UpdateCourseLessonDataAsync(request.LessonId, request.LessonName);
        logger.LogInformation("Successfully updated LessonName to: {LessonName} for LessonId: {LessonId}", 
            request.LessonName, request.LessonId);

        logger.LogInformation("UpdateLessonCommandHandler successfully completed for LessonId: {LessonId}", request.LessonId);
        return request.LessonId;
    }
}
