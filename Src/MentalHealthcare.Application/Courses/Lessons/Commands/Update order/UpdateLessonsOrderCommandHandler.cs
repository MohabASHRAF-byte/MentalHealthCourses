using MediatR;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Domain.Repositories.Course;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Lessons.Commands.Update_order;

public class UpdateLessonsOrderCommandHandler(
    ILogger<UpdateLessonsOrderCommandHandler> logger,
    ICourseLessonRepository lessonRepository,
    IUserContext userContext
) : IRequestHandler<UpdateLessonsOrderCommand>
{
    public async Task Handle(UpdateLessonsOrderCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Handling updated lessons order for course {CourseId}, section {SectionId}",
            request.CourseId, request.SectionId);

        // Authenticate and validate admin permissions
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        {
            var userDetails = currentUser == null
                ? "User is null"
                : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}";

            logger.LogWarning("Unauthorized access attempt to update lessons order. User details: {UserDetails}", userDetails);
            throw new ForBidenException("You do not have permission to update the lessons order.");
        }

        // Fetch existing lessons for the specified section
        var isOrderable = await lessonRepository
            .IsSectionOrderable(request.CourseId, request.SectionId);
        if (!isOrderable)
        {
            logger.LogError("Cannot update order: Students are already joined in course {CourseId}, section {SectionId}", request.CourseId, request.SectionId);
            throw new InvalidOperationException("You can't update this order because students are joined.");
        }

        var lessons = await lessonRepository.GetCourseLessons(request.CourseId, request.SectionId);
        if (lessons == null || lessons.Count == 0)
        {
            logger.LogWarning("No lessons found for section {SectionId} in course {CourseId}", request.SectionId, request.CourseId);
            throw new InvalidOperationException(
                $"Section {request.SectionId} in course {request.CourseId} has no lessons.");
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
                    "Updating order for Lesson ID {LessonId}: {CurrentOrder} -> {NewOrder}",
                    lesson.CourseLessonId, lesson.Order, newLessonOrder
                );
                lesson.Order = newLessonOrder;
                changesMade = true;
            }
            else
            {
                logger.LogInformation(
                    "No change for Lesson ID {LessonId}: Current Order = {CurrentOrder}",
                    lesson.CourseLessonId, lesson.Order
                );
            }
        }

        // Save changes if any updates were made
        if (changesMade)
        {
            await lessonRepository.UpdateCourseLessonsAsync(lessons,request.CourseId);
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
