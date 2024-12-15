using AutoMapper;
using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.BunnyServices.VideoContent.Video;
using MentalHealthcare.Application.Courses.LessonResources.Commands.Delete_Resource;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
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
    IMediator mediator
) : IRequestHandler<RemoveLessonCommand>
{
    public async Task Handle(RemoveLessonCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting RemoveLessonCommandHandler for LessonId: {LessonId}, CourseId: {CourseId}, SectionId: {SectionId}",
            request.LessonId, request.CourseId, request.SectionId);

        // TODO: Uncomment and implement user authorization.
        // var currentUser = userContext.GetCurrentUser();
        // if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        // {
        //     logger.LogError("Unauthorized access attempt for removing a lesson.");
        //     throw new UnauthorizedAccessException();
        // }

        // Retrieve all lessons for the given course and section.
        logger.LogInformation("Fetching lessons for CourseId: {CourseId}, SectionId: {SectionId}", 
            request.CourseId, request.SectionId);

        var lessons = await courseLessonRepository.GetCourseLessons(request.CourseId, request.SectionId);

        if (!lessons.Any())
        {
            logger.LogWarning("No lessons found for SectionId: {SectionId} in CourseId: {CourseId}", 
                request.SectionId, request.CourseId);
            throw new ResourceNotFound("Lesson", request.SectionId.ToString());
        }

        // Find the target lesson to be removed.
        var targetLesson = lessons.FirstOrDefault(lesson => lesson.CourseLessonId == request.LessonId);
        if (targetLesson == null)
        {
            logger.LogWarning("No lesson found with LessonId: {LessonId}", request.LessonId);
            throw new ResourceNotFound("Lesson", request.LessonId.ToString());
        }

        // Initialize Bunny client.
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

        var tobeDeLesson = await courseLessonRepository.GetCourseLessonByIdAsync(request.LessonId);
        if(tobeDeLesson.CourseLessonResources != null)
        {
            foreach (var resource in tobeDeLesson.CourseLessonResources)
            {
                var tobeDeletedResource = new DeleteLessonResourceCommand()
                {
                    CourseId = request.CourseId,
                    LessonId = request.LessonId,
                    ResourceId = resource.CourseLessonResourceId
                };
                await mediator.Send(tobeDeletedResource, cancellationToken);
            }
        }
        
        // Delete the target lesson from the database.
        logger.LogInformation("Deleting lesson with LessonId: {LessonId}", request.LessonId);
        await courseLessonRepository.DeleteCourseLessonAsync(targetLesson);

        // Adjust the order of the remaining lessons.
        logger.LogInformation("Adjusting order of remaining lessons for SectionId: {SectionId}", request.SectionId);
        var targetOrder = targetLesson.Order;
        var updatedLessons = lessons
            .Where(lesson => lesson.Order > targetOrder)
            .ToList();

        foreach (var lesson in updatedLessons)
        {
            lesson.Order--;
        }

        // Update the adjusted lessons in the database.
        if (updatedLessons.Any())
        {
            logger.LogInformation("Updating order for {LessonCount} remaining lessons.", updatedLessons.Count);
            await courseLessonRepository.UpdateCourseLessonsAsync(updatedLessons);
        }

        logger.LogInformation("Successfully removed lesson with LessonId: {LessonId} and updated lesson orders.", request.LessonId);
    }
}
