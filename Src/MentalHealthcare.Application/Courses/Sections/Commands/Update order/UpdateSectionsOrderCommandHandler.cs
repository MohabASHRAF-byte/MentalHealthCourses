using MediatR;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;

namespace MentalHealthcare.Application.Courses.Sections.Commands.Update_order;

public class UpdateSectionsOrderCommandHandler(
    ILogger<UpdateSectionsOrderCommandHandler> logger,
    ICourseSectionRepository sectionRepository,
    IUserContext userContext
) : IRequestHandler<UpdateSectionsOrderCommand>
{
    public async Task Handle(UpdateSectionsOrderCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Handling updated sections order for course {request.CourseId}");

        // Authenticate and validate user permissions
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        {
            var userDetails = currentUser == null
                ? "User is null"
                : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}";

            logger.LogWarning("Unauthorized access attempt to update sections order. User details: {UserDetails}", userDetails);
            throw new ForBidenException("You do not have permission to update sections order.");
        }

        try
        {
            await sectionRepository.UpdateCourseSectionsAsync(request.CourseId, request.Orders);
            logger.LogInformation("Successfully updated sections order for course {CourseId}.", request.CourseId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while updating sections order for course {CourseId}", request.CourseId);
            throw;
        }
    }
}