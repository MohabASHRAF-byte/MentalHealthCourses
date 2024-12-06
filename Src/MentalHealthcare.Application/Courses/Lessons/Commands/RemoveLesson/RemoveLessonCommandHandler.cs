using AutoMapper;
using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.BunnyServices.VideoContent.Video;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Lessons.Commands.RemoveLesson;

public class RemoveLessonCommandHandler(
    ILogger<RemoveLessonCommandHandler> logger,
    ICourseLessonRepository courseLessonRepository,
    ICourseRepository courseRepository,
    IConfiguration configuration
) : IRequestHandler<RemoveLessonCommand>
{
    public async Task Handle(RemoveLessonCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Handling RemoveLessonCommand for lesson {request.LessonId}");

        // Fetch all lessons for the specified course and section
        var lessons = await courseLessonRepository.GetCourseLessons(request.CourseId, request.SectionId);
        
        if (lessons.Count == 0)
        {
            logger.LogWarning($"No lessons found for section {request.SectionId} in course {request.CourseId}");
            throw new ResourceNotFound("Lesson", request.SectionId.ToString());
        }

        // Find the target lesson
        var targetLesson = lessons.FirstOrDefault(lesson => lesson.CourseLessonId == request.LessonId);
        
        if (targetLesson == null)
        {
            logger.LogWarning($"No lesson found with id {request.LessonId}");
            throw new ResourceNotFound("Lesson", request.LessonId.ToString());
        }
        BunnyClient bunnyClient = new BunnyClient(configuration);

        if (targetLesson.ContentType == ContentType.Video)
        {
            await bunnyClient.DeleteVideo(targetLesson.MaterielBunneyId);
        }
        else
        {
            var courseName =await courseRepository.GetCourseName(request.CourseId);
            await bunnyClient.DeleteFileAsync(targetLesson.LessonBunnyName, courseName);
        }
        // Delete the target lesson
        await courseLessonRepository.DeleteCourseLessonAsync(targetLesson);

        // Adjust orders of remaining lessons
        var targetOrder = targetLesson.Order;
        var updatedLessons = lessons
            .Where(lesson => lesson.Order > targetOrder)
            .ToList();

        foreach (var lesson in updatedLessons)
        {
            lesson.Order--;
        }

        // Update the remaining lessons in the database
        if (updatedLessons.Any())
        {
            await courseLessonRepository.UpdateCourseLessonsAsync(updatedLessons);
        }

        logger.LogInformation($"Successfully deleted lesson {request.LessonId} and updated lesson orders.");
    }
}