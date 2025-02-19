using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.BunnyServices.VideoContent.Video;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Lessons.Commands.RemoveLesson;

/// <summary>
/// Command handler to remove a lesson from a course section.
/// </summary>
public class RemoveLessonCommandHandler(
    ILogger<RemoveLessonCommandHandler> logger,
    ICourseLessonRepository courseLessonRepository,
    ICourseRepository courseRepository,
    IConfiguration configuration,
    IUserContext userContext
) : IRequestHandler<RemoveLessonCommand>
{
    public async Task Handle(RemoveLessonCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Starting RemoveLessonCommandHandler for LessonId: {LessonId}, CourseId: {CourseId}, SectionId: {SectionId}",
            request.LessonId, request.CourseId, request.SectionId);

        // Authenticate and validate admin permissions
        userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);

        // Retrieve the target lesson
        logger.LogInformation(
            "Fetching target lesson for CourseId: {CourseId}, SectionId: {SectionId}, LessonId: {LessonId}",
            request.CourseId, request.SectionId, request.LessonId);

        var targetLesson =
            await courseLessonRepository
                .GetCourseLessonByIdAsync(
                    request.CourseId, request.SectionId, request.LessonId
                );

        if (targetLesson == null)
        {
            logger.LogWarning("Lesson not found for CourseId: {CourseId}, SectionId: {SectionId}, LessonId: {LessonId}",
                request.CourseId, request.SectionId, request.LessonId);
            throw new ResourceNotFound("lesson", "درس", request.LessonId.ToString());
        }

        // Initialize Bunny client
        var bunnyClient = new BunnyClient(configuration);

        if (targetLesson.ContentType == ContentType.Video)
        {
            logger.LogInformation("Deleting video material from BunnyCDN for LessonId: {LessonId}", request.LessonId);
            await bunnyClient.DeleteVideo(targetLesson.MaterielBunneyId);
        }
        else
        {
            logger.LogInformation("Deleting PDF material from BunnyCDN for LessonId: {LessonId}", request.LessonId);
            var courseName = await courseRepository.GetCourseName(request.CourseId);
            await bunnyClient.DeleteFileAsync(targetLesson.LessonBunnyName, courseName);
        }

        // Remove lesson from the database
        logger.LogInformation(
            "Removing lesson from the database for CourseId: {CourseId}, SectionId: {SectionId}, LessonId: {LessonId}",
            request.CourseId, request.SectionId, request.LessonId);

        await courseLessonRepository.RemoveLesson(request.CourseId, request.SectionId, request.LessonId);

        logger.LogInformation(
            "Successfully removed lesson with LessonId: {LessonId} from CourseId: {CourseId}, SectionId: {SectionId}",
            request.LessonId, request.CourseId, request.SectionId);
    }
}