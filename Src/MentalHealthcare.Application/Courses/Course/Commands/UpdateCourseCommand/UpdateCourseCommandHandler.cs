using AutoMapper;
using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Course.Commands.UpdateCourseCommand;

public class UpdateCourseCommandHandler(
    ILogger<UpdateCourseCommandHandler> logger,
    ICourseRepository courseRepository,
    IUserContext userContext
) : IRequestHandler<UpdateCourseCommand>
{
    public async Task Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting course update process for: {CourseId}", request.CourseId);

        // Retrieve the current user
        userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);

        await courseRepository.UpdateCourseAsync(
            request.CourseId,
            request.Name,
            request.Price,
            request.Description,
            request.InstructorId,
            request.CategoryId,
            request.IsFree,
            request.IsFeatured,
            request.IsArchived
        );

        logger.LogInformation("Successfully updated course with CourseId: {CourseId}", request.CourseId);
    }
}