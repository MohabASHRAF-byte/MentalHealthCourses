using MediatR;
using MentalHealthcare.Application.Courses.LessonResources.Commands.Delete_Resource;
using MentalHealthcare.Application.Courses.Lessons.Commands.RemoveLesson;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Sections.Commands.Remove_Section;

public class RemoveCourseSectionCommandHandler(
    ILogger<RemoveCourseSectionCommandHandler> logger,
    ICourseSectionRepository courseSectionRepository,
    IMediator mediator
) : IRequestHandler<RemoveCourseSectionCommand>
{
    public async Task Handle(RemoveCourseSectionCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Handling RemoveCourseSectionCommand for course{request.CourseId}");
        var sections = await courseSectionRepository.GetCourseSections(request.CourseId);
        if (sections.Count == 0)
        {
            logger.LogWarning($"No course found for id {request.SectionId}");
            throw new ResourceNotFound("course ", request.SectionId.ToString());
        }

        var targetSection = sections.FirstOrDefault(section => section.CourseSectionId == request.SectionId);
        var delSection = await courseSectionRepository
            .GetCourseSectionByIdUnTrackedAsync(request.SectionId);
        foreach (var lesson in delSection.Lessons)
        {
            var delLessonResource = new RemoveLessonCommand()
            {
                CourseId = request.CourseId,
                SectionId = request.SectionId,
                LessonId = lesson.CourseLessonId,
            };
            await mediator.Send(delLessonResource, cancellationToken);
        }

        if (targetSection == null)
        {
            logger.LogWarning($"No section found for id {request.SectionId}");
            throw new ResourceNotFound("Section ", request.SectionId.ToString());
        }

        await courseSectionRepository.DeleteCourseSectionAsync(targetSection);
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
            await courseSectionRepository.UpdateCourseSectionsAsync(updatedSections);
        }
    }
}