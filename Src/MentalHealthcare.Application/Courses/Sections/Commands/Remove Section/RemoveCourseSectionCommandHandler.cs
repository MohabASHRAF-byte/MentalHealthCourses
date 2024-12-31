using MediatR;
using MentalHealthcare.Application.Courses.LessonResources.Commands.Delete_Resource;
using MentalHealthcare.Application.Courses.Lessons.Commands.RemoveLesson;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Sections.Commands.Remove_Section;

public class RemoveCourseSectionCommandHandler(
    ILogger<RemoveCourseSectionCommandHandler> logger,
    ICourseSectionRepository courseSectionRepository,
    IUserContext userContext
) : IRequestHandler<RemoveCourseSectionCommand>
{
    public async Task Handle(RemoveCourseSectionCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start handling RemoveCourseSectionCommand for CourseId: {CourseId}, SectionId: {SectionId}", request.CourseId, request.SectionId);

        // Retrieve current user and validate permissions
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        {
            var userDetails = currentUser == null
                ? "User is null"
                : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}";

            logger.LogWarning("Unauthorized access attempt to remove a course section. User details: {UserDetails}", userDetails);
            throw new ForBidenException("You do not have permission to remove this section.");
        }

        try
        {
            logger.LogInformation("Attempting to delete section {SectionId} from course {CourseId}", request.SectionId, request.CourseId);
            await courseSectionRepository.DeleteCourseSectionIfEmptyAsync(request.CourseId, request.SectionId);
            logger.LogInformation("Successfully deleted section {SectionId} from course {CourseId}", request.SectionId, request.CourseId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deleting section {SectionId} from course {CourseId}", request.SectionId, request.CourseId);
            throw;
        }
    }
}
