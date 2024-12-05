using AutoMapper;
using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.BunnyServices.VideoContent.Video;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;

namespace MentalHealthcare.Application.Courses.Materials.Commands.ConfirmUpload;

public class ConfirmUploadCommandHandler(
    IMediator mediator,
    ICourseRepository courseRepository,
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
            await mediator.Send(deleteVideo, cancellationToken);
        }
        else
        {
            var pending = await courseRepository.GetPendingUpload(request.videoId);
            var order = courseRepository.GetVideoOrder(pending.CourseId) + 1;
            var courseMaterial = mapper.Map<CourseMateriel>(pending);
            courseMaterial.IsVideo = true;
            courseMaterial.ItemOrder = order;
            await courseRepository.AddCourseMatrial(courseMaterial);
        }

        await courseRepository.DeletePending(request.videoId);
    }
}