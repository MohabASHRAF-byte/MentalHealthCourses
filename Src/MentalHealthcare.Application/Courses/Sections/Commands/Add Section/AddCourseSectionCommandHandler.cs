using AutoMapper;
using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Sections.Commands.Add_Section;

public class AddCourseSectionCommandHandler(
    ILogger<AddCourseSectionCommandHandler> logger,
    ICourseSectionRepository sectionRepository,
    IMapper mapper,
    IUserContext userContext
) : IRequestHandler<AddCourseSectionCommand, int>
{
    public async Task<int> Handle(AddCourseSectionCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start handling AddCourseSectionCommand for CourseId: {CourseId}", request.CourseId);

        // Retrieve current user and validate permissions
        var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);

        logger.LogInformation("User {UserId} authorized to add course section.", currentUser.Id);

        // Map request to CourseSection entity
        var newCourseSection = mapper.Map<CourseSection>(request);
        newCourseSection.Name = request.Name;

        logger.LogInformation("Mapped new section: {SectionName} for course {CourseId}", newCourseSection.Name,
            request.CourseId);

        try
        {
            await sectionRepository.AddCourseSection(newCourseSection);
            logger.LogInformation("Successfully added section: {SectionName} for course {CourseId}",
                newCourseSection.Name, request.CourseId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while adding section: {SectionName} for course {CourseId}",
                newCourseSection.Name, request.CourseId);
            throw;
        }

        logger.LogInformation("Returning CourseSectionId: {SectionId} for the newly added section.",
            newCourseSection.CourseSectionId);
        return newCourseSection.CourseSectionId;
    }
}