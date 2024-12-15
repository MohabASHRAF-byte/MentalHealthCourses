using AutoMapper;
using MediatR;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Sections.Commands.Add_Section;

public class AddCourseSectionCommandHandler(
    ILogger<AddCourseSectionCommandHandler> logger,
    ICourseSectionRepository sectionRepository,
    IMapper mapper
) : IRequestHandler<AddCourseSectionCommand, int>
{
    public async Task<int> Handle(AddCourseSectionCommand request, CancellationToken cancellationToken)
    {
        // ToDo: Implement Authentication logic here
        // Ensure the user has the appropriate role or permissions to add a section

        // ToDo: Add validation for request input
        // Validate the courseId, section name, and any other required fields

        var newCourseSection = mapper.Map<CourseSection>(request);
        newCourseSection.Name = request.Name;

        // ToDo: Add logging for the creation of the section
        logger.LogInformation($"Adding new section: {newCourseSection.Name} for course {request.CourseId}");

        await sectionRepository.AddCourseSection(newCourseSection);
        
        // ToDo: Return appropriate response after adding the section
        return newCourseSection.CourseSectionId;
    }
}