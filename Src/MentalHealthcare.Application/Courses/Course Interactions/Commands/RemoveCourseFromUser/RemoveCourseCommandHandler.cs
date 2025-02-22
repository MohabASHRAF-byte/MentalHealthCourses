using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Course_Interactions.Commands.RemoveCourseFromUser;

public class RemoveCourseCommandHandler(
    ILogger<RemoveCourseCommandHandler> logger,
    ICourseRepository courseRepository,
    IUserContext userContext
) : IRequestHandler<RemoveCourseCommand>
{
    public async Task Handle(RemoveCourseCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting removal of course {CourseId} for user {UserId}", request.CourseId, request.UserId);
        
        try
        {
            userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);
            logger.LogInformation("User authorized to remove course");
            
            await courseRepository.RemoveCourseFromUserAsync(request.CourseId, request.UserId);
            
            logger.LogInformation("Successfully removed course {CourseId} from user {UserId}", request.CourseId, request.UserId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to remove course {CourseId} from user {UserId}", request.CourseId, request.UserId);
            throw;
        }
    }
}