using MediatR;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.LessonResources.Commands.Update_resource_Order;

public class UpdateResourceOrderCommandHandler(
    ILogger<UpdateResourceOrderCommandHandler> logger,
    ICourseResourcesRepository courseResourcesRepository,
    IConfiguration configuration
) : IRequestHandler<UpdateResourceOrderCommand, int>
{
    public async Task<int> Handle(UpdateResourceOrderCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Handling update resource order for lesson {request.LessonId}");

        // Fetch existing resources for the lesson
        var resources = await courseResourcesRepository.GetCourseLessonResourcesByCourseIdAsync(request.LessonId);
        if (resources == null || resources.Count == 0)
        {
            logger.LogWarning($"No resources found for lesson ID {request.LessonId}");
            throw new InvalidOperationException($"Lesson {request.LessonId} has no resources.");
        }

        // Check for missing orders in the request
        var resourceIds = resources.Select(r => r.CourseLessonResourceId).ToHashSet();
        var requestResourceIds = request.Orders.Select(o => o.ResourceId).ToHashSet();

        if (!resourceIds.SetEquals(requestResourceIds))
        {
            logger.LogError("Mismatch between existing resources and provided orders in request.");
            throw new ArgumentException("The request does not contain valid orders for all resources.");
        }

        // Validate order values range
        var orderValues = request.Orders.Select(o => o.Order).OrderBy(o => o).ToList();
        if (!orderValues.SequenceEqual(Enumerable.Range(1, resources.Count)))
        {
            logger.LogError("The provided order values are not sequential starting from 1.");
            throw new ArgumentException("Order values must be sequential from 1 to the total number of resources.");
        }

        // Update resource orders and log changes
        bool changesMade = false;
        foreach (var resource in resources)
        {
            var newResourceOrder = request.Orders.First(o => o.ResourceId == resource.CourseLessonResourceId).Order;
            if (resource.ItemOrder != newResourceOrder)
            {
                logger.LogInformation(
                    $"Updating order for Resource ID {resource.CourseLessonResourceId}: {resource.ItemOrder} -> {newResourceOrder}"
                );
                resource.ItemOrder = newResourceOrder;
                changesMade = true;
            }
            else
            {
                logger.LogInformation(
                    $"No change for Resource ID {resource.CourseLessonResourceId}: Current Order = {resource.ItemOrder}"
                );
            }
        }

        // Save changes if any updates were made
        if (changesMade)
        {
            await courseResourcesRepository.UpdateCourseLessonResourcesAsync(resources);
            logger.LogInformation("Successfully updated resource order for lesson {LessonId}.", request.LessonId);
        }
        else
        {
            logger.LogInformation("No changes detected in resource order for lesson {LessonId}.", request.LessonId);
        }

        return resources.Count;  // Return the count of updated resources (or any other identifier if needed)
    }
}
