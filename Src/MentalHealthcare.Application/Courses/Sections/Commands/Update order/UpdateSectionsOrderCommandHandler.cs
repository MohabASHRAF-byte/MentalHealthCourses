using MediatR;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Sections.Commands.Update_order;

public class UpdateSectionsOrderCommandHandler(
    ILogger<UpdateSectionsOrderCommandHandler> logger,
    ICourseRepository courseRepository
) : IRequestHandler<UpdateSectionsOrderCommand>
{
    public async Task Handle(UpdateSectionsOrderCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Handling updated sections order for course {request.CourseId}");

        // Fetch existing sections
        var sections = await courseRepository.GetCourseSections(request.CourseId);
        if (sections == null || sections.Count == 0)
        {
            logger.LogWarning($"No sections found for course ID {request.CourseId}");
            throw new InvalidOperationException($"Course {request.CourseId} has no sections.");
        }

        // Check for missing orders in the request
        var sectionIds = sections.Select(s => s.CourseSectionId).ToHashSet();
        var requestSectionIds = request.Orders.Select(o => o.SectionId).ToHashSet();

        if (!sectionIds.SetEquals(requestSectionIds))
        {
            logger.LogError("Mismatch between existing sections and provided orders in request.");
            throw new ArgumentException("The request does not contain valid orders for all sections.");
        }

        // Validate orders range
        var orderValues = request.Orders.Select(o => o.Order).OrderBy(o => o).ToList();
        if (!orderValues.SequenceEqual(Enumerable.Range(1, sections.Count)))
        {
            logger.LogError("The provided order values are not sequential starting from 1.");
            throw new ArgumentException("Order values must be sequential from 1 to the total number of sections.");
        }

        // Update sections and log changes
        bool changesMade = false;
        foreach (var section in sections)
        {
            var newSectionOrder = request.Orders.First(o => o.SectionId == section.CourseSectionId).Order;
            if (section.Order != newSectionOrder)
            {
                logger.LogInformation(
                    $"Updating order for Section ID {section.CourseSectionId}: {section.Order} -> {newSectionOrder}"
                );
                section.Order = newSectionOrder;
                changesMade = true;
            }
            else
            {
                logger.LogInformation(
                    $"No change for Section ID {section.CourseSectionId}: Current Order = {section.Order}"
                );
            }
        }

        // Save changes if any updates were made
        if (changesMade)
        {
            await courseRepository.UpdateCourseSectionsAsync(sections);
            logger.LogInformation("Successfully updated sections order for course {CourseId}.", request.CourseId);
        }
        else
        {
            logger.LogInformation("No changes detected in sections order for course {CourseId}.", request.CourseId);
        }
    }
}
