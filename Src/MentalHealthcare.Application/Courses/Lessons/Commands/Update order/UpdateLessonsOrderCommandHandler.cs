using MediatR;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Lessons.Commands.Update_order;

public class UpdateLessonsOrderCommandHandler(
    ILogger<UpdateLessonsOrderCommandHandler> logger,
    ICourseRepository courseRepository
) : IRequestHandler<UpdateLessonsOrderCommand>
{
    public async Task Handle(UpdateLessonsOrderCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Handling updated lessons order for course {request.CourseId}, section {request.SectionId}");

        // Fetch existing lessons for the specified section
        var lessons = await courseRepository.GetCourseLessons(request.CourseId, request.SectionId);
        if (lessons == null || lessons.Count == 0)
        {
            logger.LogWarning($"No lessons found for section {request.SectionId} in course {request.CourseId}");
            throw new InvalidOperationException($"Section {request.SectionId} in course {request.CourseId} has no lessons.");
        }

        // Check for missing or mismatched lesson IDs in the request
        var lessonIds = lessons.Select(l => l.CourseLessonId).ToHashSet();
        var requestLessonIds = request.Orders.Select(o => o.LessonId).ToHashSet();

        if (!lessonIds.SetEquals(requestLessonIds))
        {
            logger.LogError("Mismatch between existing lessons and provided orders in request.");
            throw new ArgumentException("The request does not contain valid orders for all lessons.");
        }

        // Validate the order range
        var orderValues = request.Orders.Select(o => o.Order).OrderBy(o => o).ToList();
        if (!orderValues.SequenceEqual(Enumerable.Range(1, lessons.Count)))
        {
            logger.LogError("The provided order values are not sequential starting from 1.");
            throw new ArgumentException("Order values must be sequential from 1 to the total number of lessons.");
        }

        // Update lessons and log changes
        bool changesMade = false;
        foreach (var lesson in lessons)
        {
            var newLessonOrder = request.Orders.First(o => o.LessonId == lesson.CourseLessonId).Order;
            if (lesson.Order != newLessonOrder)
            {
                logger.LogInformation(
                    $"Updating order for Lesson ID {lesson.CourseLessonId}: {lesson.Order} -> {newLessonOrder}"
                );
                lesson.Order = newLessonOrder;
                changesMade = true;
            }
            else
            {
                logger.LogInformation(
                    $"No change for Lesson ID {lesson.CourseLessonId}: Current Order = {lesson.Order}"
                );
            }
        }

        // Save changes if any updates were made
        if (changesMade)
        {
            await courseRepository.UpdateCourseLessonsAsync(lessons);
            logger.LogInformation(
                "Successfully updated lessons order for section {SectionId} in course {CourseId}.",
                request.SectionId,
                request.CourseId
            );
        }
        else
        {
            logger.LogInformation(
                "No changes detected in lessons order for section {SectionId} in course {CourseId}.",
                request.SectionId,
                request.CourseId
            );
        }
    }
}
