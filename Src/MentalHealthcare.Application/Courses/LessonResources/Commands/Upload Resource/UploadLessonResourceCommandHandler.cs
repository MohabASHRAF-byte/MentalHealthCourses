using System.Diagnostics;
using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.LessonResources.Commands.Upload_Resource;

public class UploadLessonResourceCommandHandler(
    ILogger<UploadLessonResourceCommandHandler> logger,
    ICourseResourcesRepository courseResourcesRepository,
    ICourseRepository courseRepository,
    IConfiguration configuration,
    IUserContext userContext
) : IRequestHandler<UploadLessonResourceCommand, int>
{
    public async Task<int> Handle(UploadLessonResourceCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting resource upload process for Course ID: {CourseId}, Lesson ID: {LessonId}", 
            request.CourseId, request.LessonId);

        // Authenticate and validate admin permissions
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        {
            var userDetails = currentUser == null
                ? "User is null"
                : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}";

            logger.LogWarning("Unauthorized access attempt to upload resource. User details: {UserDetails}", userDetails);
            throw new UnauthorizedAccessException("You do not have permission to perform this action.");
        }

        logger.LogInformation("Admin access granted for user {UserId}", currentUser.Id);

        // Validate file size
        var fileSizeInMB = request.File.Length / (1 << 20); // Convert bytes to MB
        if (fileSizeInMB > Global.CourseRecourseSize)
        {
            logger.LogError("File size ({FileSize}MB) exceeds the limit of {Limit}MB.", fileSizeInMB, Global.CourseRecourseSize);
            throw new ArgumentException($"The file {request.FileName} is too large.");
        }

        // Validate content type
        if (!Enum.IsDefined(typeof(ContentType), request.ContentType))
        {
            logger.LogWarning("Invalid content type: {ContentType}", request.ContentType);
            throw new ArgumentException($"Invalid value for {nameof(request.ContentType)}: {request.ContentType}");
        }

        // Initialize Bunny client
        BunnyClient bunnyClient = new BunnyClient(configuration);

        // Retrieve course details
        var course = await courseRepository.GetMinimalCourseByIdAsync(request.CourseId);
        if (course == null)
        {
            logger.LogError("Course with ID {CourseId} not found.", request.CourseId);
            throw new ArgumentException($"Course with ID {request.CourseId} not found.");
        }

        // Prepare resource entity
        var courseResource = new CourseLessonResource
        {
            ContentType = request.ContentType,
            Title = request.FileName,
            CourseLessonId = request.LessonId,
            Url = string.Empty,
            BunnyId = string.Empty,
            BunnyPath = string.Empty
        };

        // Save resource entry to database
        await courseResourcesRepository.AddCourseLessonResourceAsync(courseResource);

        // Construct file path and file name
        var filePath = $"{Global.CourseRecoursesPath}/{course.Name}";
        var fileName = $"{courseResource.CourseLessonResourceId}{request.ContentType switch
        {
            ContentType.Video => ContentExtension.Video,
            ContentType.Image => ContentExtension.Image,
            ContentType.Audio => ContentExtension.Audio,
            ContentType.Pdf => ContentExtension.Pdf,
            ContentType.Text => ContentExtension.Text,
            ContentType.Zip => ContentExtension.Zip,
            _ => throw new ArgumentOutOfRangeException(nameof(request.ContentType), request.ContentType, "Unknown content type")
        }}";

        logger.LogInformation("Uploading file: {FileName} to path: {FilePath}", fileName, filePath);

        // Upload file to BunnyCDN
        var uploadResponse = await bunnyClient.UploadFileAsync(request.File, fileName, filePath);

        // Update resource entity with upload details
        courseResource.Url = uploadResponse.Url;
        courseResource.BunnyId = fileName;
        courseResource.BunnyPath = filePath;
        await courseResourcesRepository.SaveChangesAsync();

        logger.LogInformation("Resource upload completed successfully. Resource ID: {ResourceId}, URL: {ResourceUrl}", 
            courseResource.CourseLessonResourceId, courseResource.Url);

        return courseResource.CourseLessonResourceId;
    }
}
