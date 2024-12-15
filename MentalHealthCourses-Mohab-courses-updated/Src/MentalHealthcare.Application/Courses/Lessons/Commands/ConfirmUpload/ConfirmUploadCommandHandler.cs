using AutoMapper;
using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.BunnyServices.VideoContent.Video;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Lessons.Commands.ConfirmUpload;

/// <summary>
/// Command handler for confirming or rejecting an upload.
/// </summary>
public class ConfirmUploadCommandHandler(
    IMediator mediator,
    ICourseRepository courseRepository,
    ICourseLessonRepository courseLessonRepository,
    IConfiguration configuration,
    IMapper mapper,
    ILogger<ConfirmUploadCommandHandler> logger
) : IRequestHandler<ConfirmUploadCommand,int>
{
    public async Task<int> Handle(ConfirmUploadCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling ConfirmUploadCommand for VideoId: {VideoId}, Confirmed: {Confirmed}", 
            request.videoId, request.Confirmed);

        BunnyClient bunny = new(configuration);
        var courseLessonId = -1;
        if (!request.Confirmed)
        {
            logger.LogInformation("Video confirmation rejected. Deleting video with VideoId: {VideoId}", request.videoId);
            
            // Call BunnyCDN to delete the video
            var deleteVideo = await bunny.DeleteVideo(request.videoId);
            if (!deleteVideo.Success)
            {
                logger.LogError("Failed to delete video with VideoId: {VideoId}.", request.videoId);
                throw new Exception("Failed to delete video. Try again.");
            }

            logger.LogInformation("Successfully deleted video with VideoId: {VideoId}", request.videoId);
        }
        else
        {
            logger.LogInformation("Video confirmation accepted. Fetching pending upload for VideoId: {VideoId}", 
                request.videoId);

            // Retrieve the pending video upload
            var pending = await courseRepository.GetPendingUpload(request.videoId);
            if (pending == null)
            {
                logger.LogError("No pending upload found for VideoId: {VideoId}", request.videoId);
                throw new Exception("Pending upload not found.");
            }

            logger.LogInformation("Mapping pending upload to CourseLesson for VideoId: {VideoId}", request.videoId);
            var courseLesson = mapper.Map<CourseLesson>(pending);
            courseLesson.ContentType = ContentType.Video;
            courseLesson.LessonName = pending.Title;
            courseLesson.MaterielBunneyId = pending.PendingVideoUploadId;

            // Save the course lesson
            logger.LogInformation("Adding CourseLesson for VideoId: {VideoId}", request.videoId);
            await courseLessonRepository.AddCourseLesson(courseLesson);
            logger.LogInformation("Successfully added CourseLesson for VideoId: {VideoId}", request.videoId);
            courseLessonId = courseLesson.CourseLessonId;
        }

        // Delete the pending upload from the repository
        logger.LogInformation("Deleting pending upload for VideoId: {VideoId}", request.videoId);
        await courseRepository.DeletePending(request.videoId);
        logger.LogInformation("Successfully handled ConfirmUploadCommand for VideoId: {VideoId}", request.videoId);
        return courseLessonId;
    }
}
