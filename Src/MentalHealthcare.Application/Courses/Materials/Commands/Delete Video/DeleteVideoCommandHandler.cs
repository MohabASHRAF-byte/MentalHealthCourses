using MediatR;
using MentalHealthcare.Application.Courses.Materials.Commands.CreateVideo;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Materials.Commands.Delete_Video;

public class DeleteVideoCommandHandler(
    ILogger<CreateVideoCommandHandler> logger,
    ICourseRepository courseRepository,
    IConfiguration configuration,
    IMediator mediator,
    IUserContext userContext,
    IAdminRepository adminRepository
    ):IRequestHandler<DeleteVideoCommand>
{
    public async Task Handle(DeleteVideoCommand request, CancellationToken cancellationToken)
    {
        //todo 
        //add auth 
        var material = await courseRepository.GetCourseMaterielByIdAsync(request.MaterialId);
        // material.
    }
}