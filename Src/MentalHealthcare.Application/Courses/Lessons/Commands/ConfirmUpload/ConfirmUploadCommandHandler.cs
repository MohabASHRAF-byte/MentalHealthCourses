using AutoMapper;
using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.BunnyServices.VideoContent.Video;
using MentalHealthcare.Application.Courses.Materials.Commands.ConfirmUpload;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Configuration;

namespace MentalHealthcare.Application.Courses.Lessons.Commands.ConfirmUpload;

public class ConfirmUploadCommandHandler(
    IMediator mediator,
    ICourseRepository courseRepository,
    ICourseLessonRepository courseLessonRepository,
    IConfiguration configuration,
    IMapper mapper
) : IRequestHandler<ConfirmUploadCommand>
{
    public async Task Handle(ConfirmUploadCommand request, CancellationToken cancellationToken)
    {
        BunnyClient bunny = new(configuration);
        if (!request.Confirmed)
        {
            var deleteVideo = await bunny.DeleteVideo(request.videoId);
            if (!deleteVideo.Success)
            {
                throw new Exception("Failed to delete video Try Again");
            }
        }
        else
        {
            var pending = await courseRepository.GetPendingUpload(request.videoId);
            var courseLesson = mapper.Map<CourseLesson>(pending);
            courseLesson.ContentType = ContentType.Video;
            await courseLessonRepository.AddCourseLesson(courseLesson);
        }

        await courseRepository.DeletePending(request.videoId);
    }
}