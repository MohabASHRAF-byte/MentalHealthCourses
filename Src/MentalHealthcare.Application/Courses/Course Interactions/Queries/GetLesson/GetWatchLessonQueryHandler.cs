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
        logger.LogInformation("Starting Handle method for GetWatchLessonQuery with LessonId: {LessonId}",
            request.LessonId);

        var currentUser = userContext.EnsureAuthorizedUser([UserRoles.User], logger);

        logger.LogInformation("User with SysUserId: {SysUserId} is attempting to fetch LessonId: {LessonId}",
            currentUser.SysUserId, request.LessonId);

        try
        {
            var courseLesson = await courseInteractionsRepository.GetLessonAsync(request.CourseId, request.LessonId,
                (int)currentUser.SysUserId!);

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