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
        userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);
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