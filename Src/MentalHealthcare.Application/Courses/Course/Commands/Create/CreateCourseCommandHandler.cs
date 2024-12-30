using AutoMapper;
using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.BunnyServices.VideoContent.Collection.Add;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Course.Commands.Create;

/// <summary>
/// Processes the creation of a new course based on the given command.
/// </summary>
public class CreateCourseCommandHandler(
    ILogger<CreateCourseCommandHandler> logger,
    IMapper mapper,
    ICourseRepository courseRepository,
    IConfiguration configuration,
    IUserContext userContext
) : IRequestHandler<CreateCourseCommand, CreateCourseCommandResponse>
{
    /// <summary>
    /// Handles the course creation process.
    /// </summary>
    /// <param name="request">The command containing the details required to create a new course.</param>
    /// <param name="cancellationToken">Token to observe for cancellation requests.</param>
    /// <returns>A task that represents the result of the operation, containing the ID of the created course.</returns>
    /// <exception cref="Exception">Thrown if there is an error during the creation of the course or collection.</exception>
    public async Task<CreateCourseCommandResponse> Handle(CreateCourseCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting course creation process for: {CourseName}", request.Name);

        // Retrieve the current user
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        {
            logger.LogWarning(
                "Unauthorized access attempt to create a course. User information: {UserDetails}",
                currentUser == null
                    ? "User is null"
                    : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}"
            );
            throw new ForBidenException("You do not have permission to create a course.");
        }

        logger.LogInformation("Uploading thumbnail for course: {CourseName}", request.Name);
        var bunny = new BunnyClient(configuration);

        logger.LogInformation("Creating video folder for course: {CourseName}", request.Name);
        var collectionId = await bunny.CreateVideoFolderAsync(request.Name);
        if (collectionId == null)
        {
            logger.LogError("Failed to create video folder for course: {CourseName}", request.Name);
            throw new TryAgain("Failed to create video folder. Please try again.");
        }

        logger.LogInformation("Video folder created successfully with ID: {CollectionId}", collectionId);

        var course = mapper.Map<Domain.Entities.Course>(request);
        course.CollectionId = collectionId;
        // course.ThumbnailUrl = thumbnailResponse.Url;
        course.ThumbnailName = $"{request.Name}.jpeg";
        course.IsFree = request.Price == 0;
        course.IsArchived = false;
        course.CreatedAt = DateTime.UtcNow;
        logger.LogInformation("Inserting course {CourseName} into the database", request.Name);
        var courseId = await courseRepository.CreateAsync(course, request.CategoryId);

        logger.LogInformation("Course created successfully with ID: {CourseId}", courseId);

        return new CreateCourseCommandResponse
        {
            CourseId = courseId
        };
    }
}