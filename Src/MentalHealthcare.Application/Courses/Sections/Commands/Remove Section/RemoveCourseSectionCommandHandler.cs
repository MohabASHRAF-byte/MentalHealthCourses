using MediatR;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Sections.Commands.Remove_Section;

public class RemoveCourseSectionCommandHandler(
    ILogger<RemoveCourseSectionCommandHandler> logger,
    ICourseRepository courseRepository
) : IRequestHandler<RemoveCourseSectionCommand>
{
    public async Task Handle(RemoveCourseSectionCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Handling RemoveCourseSectionCommand for course{request.CourseId}");
        var sections = await courseRepository.GetCourseSections(request.CourseId);
        if (sections.Count == 0)
        {
            logger.LogWarning($"No course found for id {request.SectionId}");
            throw new ResourceNotFound("course ", request.SectionId.ToString());
        }

        var targetSection = sections.FirstOrDefault(section => section.CourseSectionId == request.SectionId);
        if (targetSection == null)
        {
            logger.LogWarning($"No section found for id {request.SectionId}");
            throw new ResourceNotFound("Section ", request.SectionId.ToString());
        }

        await courseRepository.DeleteCourseSectionAsync(targetSection);
        // Adjust orders of remaining sections
        var targetOrder = targetSection.Order;
        var updatedSections = sections
            .Where(section => section.Order > targetOrder)
            .ToList();

        foreach (var section in updatedSections)
        {
            section.Order--;
        }

        if (updatedSections.Any())
        {
            await courseRepository.UpdateCourseSectionsAsync(updatedSections);
        }

        
    }
}