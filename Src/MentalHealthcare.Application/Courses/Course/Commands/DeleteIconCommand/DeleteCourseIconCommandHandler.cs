using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Course.Commands.DeleteIconCommand;

public class DeleteCourseIconCommandHandler(
    ILogger<DeleteCourseIconCommandHandler> logger,
    ICourseRepository courseRepository,
    IConfiguration configuration,
    IUserContext userContext,
    ILocalizationService localizationService
) : IRequestHandler<DeleteCourseIconCommand>
{
    public async Task Handle(DeleteCourseIconCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start handling DeleteCourseIconCommand for CourseId: {CourseId}", request.CourseId);

        // Retrieve current user and validate permissions
        var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin],logger);

        logger.LogInformation("User {UserId} authorized to delete course icon.", currentUser.Id);

        // Retrieve course information
        logger.LogInformation("Fetching course details for CourseId: {CourseId}", request.CourseId);
        var course = await courseRepository.GetMinimalCourseByIdAsync(request.CourseId);
        if (course == null)
        {
            logger.LogError("Course with ID {CourseId} not found.", request.CourseId);
            throw new ResourceNotFound(nameof(course),"دورة تدريبية", request.CourseId.ToString());
        }

        logger.LogInformation("Course found: {CourseName} (ID: {CourseId})", course.Name, request.CourseId);

        // Check if the course has an icon to delete
        if (string.IsNullOrEmpty(course.IconName))
        {
            logger.LogInformation("Course with ID {CourseId} does not have an icon to delete.", request.CourseId);
            throw new BadHttpRequestException(
                string.Format(
                    localizationService.GetMessage("CourseIconMissing"),
                    request.CourseId
                )
            );
        }

        // Delete icon using Bunny service
        logger.LogInformation("Deleting icon for CourseId: {CourseId}", request.CourseId);
        var bunny = new BunnyClient(configuration);
        var deleteResponse = await bunny.DeleteFileAsync(course.IconName, Global.CourseIconsDirectory);

        if (!deleteResponse.IsSuccessful)
        {
            logger.LogWarning("Failed to delete icon for CourseId: {CourseId}. Error: {ErrorMessage}",
                request.CourseId, deleteResponse.Message);
            throw new BadHttpRequestException(
                localizationService.GetMessage("FailedToDeleteIcon", "Failed to delete icon. Please try again.")
            );
        }

        logger.LogInformation("Icon deleted successfully for CourseId: {CourseId}", request.CourseId);

        // Clear icon information in the database
        logger.LogInformation("Clearing icon information for CourseId: {CourseId} in the database.", request.CourseId);
        course.IconUrl = null;
        course.IconName = null;
        await courseRepository.SaveChangesAsync();

        logger.LogInformation("Successfully handled DeleteCourseIconCommand for CourseId: {CourseId}",
            request.CourseId);
    }
}