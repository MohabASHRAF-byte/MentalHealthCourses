using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.BunnyServices.VideoContent.Video.CreateVideo;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;

namespace MentalHealthcare.Application.Videos.Commands.CreateVideo;

public class CreateVideoCommandHandler(
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
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
            throw new UnauthorizedAccessException();
        var admin = await adminRepository.GetAdminByIdentityAsync(currentUser.Id);
        var course = await courseRepository.GetCourseByIdAsync(request.CourseId);
        var libId = configuration["BunnyCdn:LibraryId"]!;
        var addVideoCommand = new AddVideoCommand
        {
            CollectionId = course.CollectionId,
            LibraryId = libId,
            VideoName = request.VideoName
        };
        var videoId = await mediator.Send(addVideoCommand, cancellationToken);
        //todo handle this error
        if (videoId == null)
            throw new Exception("UnExpected Error");
        var bunny = new BunnyClient(configuration);
        var ret = bunny.GenerateSignature(libId, course.CollectionId, videoId);
        await courseRepository.AddPendingUpload(new()
        {
            CreatedDate = DateTime.UtcNow,
            PendingVideoUploadId = videoId,
            CourseId = request.CourseId,
            Title = request.VideoName,
            Description = request.Description,
            Url = $"https://iframe.mediadelivery.net/play/{libId}/{videoId}",
            AdminId = admin.AdminId
        });
        return ret;
    }
}