using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.BunnyServices.VideoContent.Video;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Application.Videos.Commands.CreateVideo;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Materials.Commands.CreateVideo;

public class CreateVideoCommandHandler(
    ILogger<CreateVideoCommandHandler> logger,
    ICourseRepository courseRepository,
    IConfiguration configuration,
    IMediator mediator,
    IUserContext userContext,
    IAdminRepository adminRepository
) : IRequestHandler<CreateVideoCommand, CreateVideoCommandResponse>
{
    public async Task<CreateVideoCommandResponse> Handle(CreateVideoCommand request,
        CancellationToken cancellationToken)
    {
        //todo 
        //remove comment
        // var currentUser = userContext.GetCurrentUser();
        // if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        //     throw new UnauthorizedAccessException();
        var lesson =await courseRepository.GetCourseLessonByIdAsync(request.LessonId);
        if (lesson.CourseId != request.CourseId || lesson.CourseSectionId != request.SectionId)
        {
            logger.LogWarning($"Course {request.CourseId} or Section {request.SectionId} not found");
            throw new ArgumentException("Course or section id not found");
        }
        // var admin = await adminRepository.GetAdminByIdentityAsync(currentUser.Id);

        var course = await courseRepository.GetCourseByIdAsync(request.CourseId);
        var bunny = new BunnyClient(configuration);
        var videoId = await bunny.CreateVideoAsync(request.VideoName, course.CollectionId);
        //todo
        // handle this error
        if (videoId == null)
            throw new Exception("UnExpected Error");
        var ret = bunny.GenerateSignature(course.CollectionId, videoId);
        await courseRepository.AddPendingUpload(new()
        {
            CreatedDate = DateTime.UtcNow,
            PendingVideoUploadId = videoId,
            CourseId = request.CourseId,
            Title = request.VideoName,
            Description = request.Description,
            CourseLessonId = request.LessonId,
            CourseSectionId = request.SectionId,
            Url = bunny.GenerateVideoFrameUrl(videoId),
            AdminId = 1
        });
        return ret;
    }
}