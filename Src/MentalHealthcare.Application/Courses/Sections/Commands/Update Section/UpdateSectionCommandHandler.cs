using AutoMapper;
using MediatR;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Sections.Commands.Update_Section;

public class UpdateSectionCommandHandler(
    ILogger<UpdateSectionCommandHandler> logger,
    IMapper mapper,
    ICourseSectionRepository courseSectionRepository
) : IRequestHandler<UpdateSectionCommand, int>
{
    public async Task<int> Handle(UpdateSectionCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Updated section {request.SectionId}");
        var section = await courseSectionRepository.GetCourseSectionByIdAsync(request.SectionId);
        section.Name = request.SectionName;
        await courseSectionRepository.SaveChangesAsync();
        return request.SectionId;
    }
}