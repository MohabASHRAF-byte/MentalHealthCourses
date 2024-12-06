using AutoMapper;
using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.BunnyServices.VideoContent.Video;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Application.Videos.Commands.CreateVideo;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Lessons.Commands.CreateVideo;

/// <summary>
/// Command handler for creating a video lesson.
/// </summary>
public class CreateVideoCommandHandler(
    ILogger<CreateVideoCommandHandler> logger,
    ICourseRepository courseRepository,
    ICourseSectionRepository courseSectionRepository,
    IConfiguration configuration,
    IMapper mapper
) : IRequestHandler<CreateVideoCommand, CreateVideoCommandResponse>
{
    public async Task<CreateVideoCommandResponse> Handle(CreateVideoCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Start handling CreateVideoCommand for CourseId: {CourseId}, SectionId: {SectionId}",
            request.CourseId, request.CourseSectionId);

        // TODO: Ensure proper authorization for the current user
        // var currentUser = userContext.GetCurrentUser();
        // if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        //     throw new UnauthorizedAccessException();

        // Validate that the course section exists
        logger.LogInformation("Validating existence of CourseId: {CourseId}, SectionId: {SectionId}",
            request.CourseId, request.CourseSectionId);
        await courseSectionRepository.IsExistAsync(request.CourseId, request.CourseSectionId);

        // TODO: Link admin with the action and retrieve admin info
        // var admin = await adminRepository.GetAdminByIdentityAsync(currentUser.Id);

        // Retrieve the collection ID for the course
        logger.LogInformation("Retrieving CollectionId for CourseId: {CourseId}", request.CourseId);
        var collectionId = await courseRepository.GetCourseCollectionId(request.CourseId);

        // Call BunnyCDN API to create the video
        logger.LogInformation(
            "Calling BunnyCDN API to create video with Title: {Title} in CollectionId: {CollectionId}",
            request.Title, collectionId);
        var bunny = new BunnyClient(configuration);
        var videoId = await bunny.CreateVideoAsync(request.Title, collectionId);

        // TODO: Handle video creation errors more gracefully
        if (videoId == null)
        {
            logger.LogError("Failed to create video on BunnyCDN for Title: {Title}", request.Title);
            throw new Exception("Unexpected error occurred while creating the video.");
        }

        logger.LogInformation("Successfully created video on BunnyCDN with VideoId: {VideoId}", videoId);

        // Generate a secure signature for the video
        logger.LogInformation("Generating signature for VideoId: {VideoId} in CollectionId: {CollectionId}",
            videoId, collectionId);
        var ret = bunny.GenerateSignature(collectionId, videoId);

        // Map request data to PendingVideoUpload entity
        logger.LogInformation("Mapping request data to PendingVideoUpload entity for VideoId: {VideoId}", videoId);
        var pendingUpload = mapper.Map<PendingVideoUpload>(request);
        pendingUpload.CreatedDate = DateTime.UtcNow;
        pendingUpload.PendingVideoUploadId = videoId;
        pendingUpload.Url = bunny.GenerateVideoFrameUrl(videoId);
        pendingUpload.AdminId = 1;

        // Save the pending upload in the repository
        logger.LogInformation("Saving PendingVideoUpload entity to the repository for VideoId: {VideoId}", videoId);
        await courseRepository.AddPendingUpload(pendingUpload);

        logger.LogInformation("Successfully handled CreateVideoCommand for VideoId: {VideoId}", videoId);
        return ret;
    }
}