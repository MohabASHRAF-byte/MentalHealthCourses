using AutoMapper;
using MediatR;
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
    ICourseLessonRepository courseLessonRepository
) : IRequestHandler<UpdateLessonCommand, int>
{
    public async Task<int> Handle(UpdateLessonCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting UpdateLessonCommandHandler for LessonId: {LessonId}, LessonName: {LessonName}",
            request.LessonId, request.LessonName);

        // TODO: Uncomment and implement user authorization.
        // var currentUser = userContext.GetCurrentUser();
        // if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        // {
        //     logger.LogError("Unauthorized access attempt for updating lesson data.");
        //     throw new UnauthorizedAccessException();
        // }

        logger.LogInformation("Validating if the lesson exists with LessonId: {LessonId}", request.LessonId);

        // Perform the update.
        await courseLessonRepository.UpdateCourseLessonDataAsync(request.LessonId, request.LessonName);
        logger.LogInformation("Successfully updated LessonName to: {LessonName} for LessonId: {LessonId}", 
            request.LessonName, request.LessonId);

        logger.LogInformation("UpdateLessonCommandHandler successfully completed for LessonId: {LessonId}", request.LessonId);
        return request.LessonId;
    }
}