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

namespace MentalHealthcare.Application.Courses.LessonResources.Commands.Delete_Resource;

public class DeleteLessonResourceCommandHandler(
    ILogger<DeleteLessonResourceCommandHandler> logger,
    ICourseResourcesRepository courseResourcesRepository,
    IConfiguration configuration,
    IUserContext userContext,
    ILocalizationService localizationService
) : IRequestHandler<DeleteLessonResourceCommand>
{
    public async Task Handle(DeleteLessonResourceCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Initiating deletion for resource with ID: {ResourceId}", request.ResourceId);

        // Authenticate and validate admin permissions
        userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);

        // Retrieve the resource to be deleted
        var resourceToDelete = await courseResourcesRepository.GetCourseLessonResourceByIdAsync(request.ResourceId);
        if (resourceToDelete == null)
        {
            logger.LogWarning("Resource with ID: {ResourceId} not found", request.ResourceId);
            throw new ResourceNotFound(
                "Resource",
                "مورد",
                request.ResourceId.ToString());
        }

        logger.LogInformation(
            "Resource found. Proceeding with BunnyCDN file deletion. BunnyId: {BunnyId}, Path: {BunnyPath}",
            resourceToDelete.BunnyId, resourceToDelete.BunnyPath);

        // Initialize BunnyCDN client and delete the file
        var bunny = new BunnyClient(configuration);
        var response = await bunny.DeleteFileAsync(resourceToDelete.BunnyId, resourceToDelete.BunnyPath);

        if (!response.IsSuccessful)
        {
            logger.LogError("Failed to delete resource on BunnyCDN. BunnyId: {BunnyId}, Path: {BunnyPath}",
                resourceToDelete.BunnyId, resourceToDelete.BunnyPath);
            throw new BadHttpRequestException(
                localizationService.GetMessage("ResourceDeletionFailed")
            );
        }

        logger.LogInformation("Successfully deleted resource from BunnyCDN. BunnyId: {BunnyId}",
            resourceToDelete.BunnyId);

        // Adjust the order of remaining resources
        var allResources =
            await courseResourcesRepository.GetCourseLessonResourcesByCourseIdAsync(resourceToDelete.CourseLessonId);
        if (allResources == null || allResources.Count == 0)
        {
            logger.LogWarning("No resources found for lesson ID {LessonId}", resourceToDelete.CourseLessonId);
            throw new BadHttpRequestException(
                string.Format(
                    localizationService.GetMessage("LessonNoResourcesToUpdate",
                        "Lesson {0} has no resources to update."),
                    resourceToDelete.CourseLessonId
                )
            );
        }

        var targetOrder = resourceToDelete.ItemOrder;
        var updatedResources = allResources
            .Where(resource => resource.ItemOrder > targetOrder)
            .ToList();

        foreach (var resource in updatedResources)
        {
            resource.ItemOrder--;
        }

        // Delete the resource entry from the database
        await courseResourcesRepository.DeleteCourseLessonResourceAsync(resourceToDelete);
        logger.LogInformation("Deleted resource entry from database. Resource ID: {ResourceId}", request.ResourceId);

        // Update the order for remaining resources
        if (updatedResources.Any())
        {
            await courseResourcesRepository.UpdateCourseLessonResourcesAsync(updatedResources);
            logger.LogInformation("Successfully adjusted order for remaining resources in lesson {LessonId}",
                resourceToDelete.CourseLessonId);
        }

        logger.LogInformation("Resource with ID: {ResourceId} successfully deleted and orders updated.",
            request.ResourceId);
    }
}