using AutoMapper;
using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Sections.Commands.Update_Section;

public class UpdateSectionCommandHandler(
    ILogger<UpdateSectionCommandHandler> logger,
    IMapper mapper,
    IUserContext userContext,
    ICourseSectionRepository courseSectionRepository
) : IRequestHandler<UpdateSectionCommand, int>
{
    public async Task<int> Handle(UpdateSectionCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start handling UpdateSectionCommand for SectionId: {SectionId}", request.SectionId);

        // Retrieve current user and validate permissions
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        {
            var userDetails = currentUser == null
                ? "User is null"
                : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}";

            logger.LogWarning("Unauthorized access attempt to update a course section. User details: {UserDetails}",
                userDetails);
            throw new ForBidenException("You do not have permission to update this section.");
        }

        logger.LogInformation("Fetching section {SectionId} for update", request.SectionId);
        var section =
            await courseSectionRepository
                .GetCourseSectionByIdAsync(
                    request.CourseId,
                    request.SectionId
                );
        if (section == null)
        {
            logger.LogWarning("Section with ID {SectionId} not found", request.SectionId);
            throw new ResourceNotFound(nameof(CourseSection), request.SectionId.ToString());
        }

        logger.LogInformation("Updating section {SectionId} with new name: {SectionName}", request.SectionId,
            request.SectionName);
        section.Name = request.SectionName;

        await courseSectionRepository.SaveChangesAsync();
        logger.LogInformation("Successfully updated section {SectionId}", request.SectionId);

        return request.SectionId;
    }
}