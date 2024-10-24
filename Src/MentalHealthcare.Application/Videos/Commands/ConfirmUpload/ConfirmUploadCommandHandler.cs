using MediatR;
using MentalHealthcare.Application.BunnyServices.VideoContent.Video.Delete;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;

namespace MentalHealthcare.Application.Videos.Commands.ConfirmUpload;

public class ConfirmUploadCommandHandler(
    IMediator mediator,
    ICourseRepository courseRepository
) : IRequestHandler<ConfirmUploadCommand>
{
    public async Task Handle(ConfirmUploadCommand request, CancellationToken cancellationToken)
    {
        if (!request.Confirmed)
        {
            var deleteVideo = new DeleteVideoCommand
            {
                VideoId = request.videoId
            };
            await mediator.Send(deleteVideo, cancellationToken);
        }
        else
        {
            var pending = await courseRepository.GetPendingUpload(request.videoId);
            var order = courseRepository.GetVideoOrder(pending.CourseId) + 1;
            var courseMatrial = new CourseMateriel()
            {
                CourseId = pending.CourseId,
                Description = pending.Description,
                AdminId = pending.AdminId,
                Title = pending.Title,
                Url = pending.Url,
                IsVideo = true,
                ItemOrder = order,
            };
            await courseRepository.AddCourseMatrial(courseMatrial);
        }
        await courseRepository.DeletePending(request.videoId);
    }
}
