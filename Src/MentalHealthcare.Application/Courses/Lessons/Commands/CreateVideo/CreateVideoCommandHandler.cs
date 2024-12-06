using AutoMapper;
using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.BunnyServices.VideoContent.Video;
using MentalHealthcare.Application.Courses.Materials.Commands.CreateVideo;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Application.Videos.Commands.CreateVideo;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Lessons.Commands.CreateVideo;

// check section exist 
// call bunny api 
// add pending upload 
public class CreateVideoCommandHandler(
    ILogger<CreateVideoCommandHandler> logger,
    ICourseRepository courseRepository,
    ICourseSectionRepository courseSectionRepository,
    IConfiguration configuration,
    IMapper mapper,
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

        await courseSectionRepository.IsExistAsync(request.CourseId, request.SectionId);

        // var admin = await adminRepository.GetAdminByIdentityAsync(currentUser.Id);

        var collectionId = await courseRepository.GetCourseCollectionId(request.CourseId);
        var bunny = new BunnyClient(configuration);
        var videoId = await bunny.CreateVideoAsync(request.VideoName, collectionId);
        //todo
        // handle this error
        if (videoId == null)
            throw new Exception("UnExpected Error");
        var ret = bunny.GenerateSignature(collectionId, videoId);
        var pendingUpload = mapper.Map<PendingVideoUpload>(request);
        pendingUpload.CreatedDate = DateTime.Now;
        pendingUpload.PendingVideoUploadId = videoId;
        pendingUpload.Url = bunny.GenerateVideoFrameUrl(videoId);
        pendingUpload.AdminId = 1;
        await courseRepository.AddPendingUpload(pendingUpload);
        return ret;
    }
}