using MediatR;
using MentalHealthcare.Application.Courses.LessonResources.Commands.Update_resource_Order;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.LessonResources.Commands.Update_Resource_Order;

public class UpdateResourceOrderCommandHandler(
    ILogger<UpdateResourceOrderCommandHandler> logger,
    ICourseResourcesRepository courseResourcesRepository,
    IUserContext userContext,
    ILocalizationService localizationService
) : IRequestHandler<UpdateResourceOrderCommand, int>
{
    public async Task<int> Handle(UpdateResourceOrderCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting UpdateResourceOrderCommand for Lesson ID: {LessonId}", request.LessonId);

        // Authenticate and validate admin permissions
        userContext.EnsureAuthorizedUser([UserRoles.Admin],logger);
        // Fetch existing resources for the lesson
        var resources = await courseResourcesRepository.GetCourseLessonResourcesByCourseIdAsync(request.LessonId);
        if (resources == null || resources.Count == 0)
        {
            logger.LogWarning("No resources found for Lesson ID: {LessonId}", request.LessonId);
            throw new BadHttpRequestException(
                string.Format(
                    localizationService.GetMessage("LessonHasNoResources", "Lesson {0} has no resources."),
                    request.LessonId
                )
            );
        }

        logger.LogInformation("Found {ResourceCount} resources for Lesson ID: {LessonId}", resources.Count, request.LessonId);

        // Check for missing orders in the request
        var resourceIds = resources.Select(r => r.CourseLessonResourceId).ToHashSet();
        var requestResourceIds = request.Orders.Select(o => o.ResourceId).ToHashSet();

        if (!resourceIds.SetEquals(requestResourceIds))
        {
            logger.LogError("Mismatch between existing resources and provided orders in the request.");
            throw new BadHttpRequestException(
                localizationService.GetMessage("InvalidOrdersForResources")
            );
        }

        // Validate order values range
        var orderValues = request.Orders.Select(o => o.Order).OrderBy(o => o).ToList();
        if (!orderValues.SequenceEqual(Enumerable.Range(1, resources.Count)))
        {
            logger.LogError("The provided order values are not sequential starting from 1.");
            throw new BadHttpRequestException(
                localizationService.GetMessage("OrderValuesMustBeSequential")
            );
        }

        // Update resource orders and log changes
        bool changesMade = false;
        foreach (var resource in resources)
        {
            var newResourceOrder = request.Orders.First(o => o.ResourceId == resource.CourseLessonResourceId).Order;
            if (resource.ItemOrder != newResourceOrder)
            {
                logger.LogInformation(
                    "Updating order for Resource ID {ResourceId}: {CurrentOrder} -> {NewOrder}",
                    resource.CourseLessonResourceId, resource.ItemOrder, newResourceOrder
                );
                resource.ItemOrder = newResourceOrder;
                changesMade = true;
            }
            else
            {
                logger.LogInformation(
                    "No change for Resource ID {ResourceId}: Current Order = {CurrentOrder}",
                    resource.CourseLessonResourceId, resource.ItemOrder
                );
            }
        }

        // Save changes if any updates were made
        if (changesMade)
        {
            await courseResourcesRepository.UpdateCourseLessonResourcesAsync(resources);
            logger.LogInformation("Successfully updated resource order for Lesson ID: {LessonId}", request.LessonId);
        }
        else
        {
            logger.LogInformation("No changes detected in resource order for Lesson ID: {LessonId}", request.LessonId);
        }

        return resources.Count;  // Return the count of updated resources
    }
}
