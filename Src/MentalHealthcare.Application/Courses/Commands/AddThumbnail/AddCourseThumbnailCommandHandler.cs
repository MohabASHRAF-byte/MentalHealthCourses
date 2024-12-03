using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.BunnyServices.Files.UploadFile;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Commands.AddThumbnail;

public class AddCourseThumbnailCommandHandler(
    ILogger<AddCourseThumbnailCommandHandler> logger,
    ICourseRepository courseRepository,
    IMediator mediator,
    IConfiguration configuration
) : IRequestHandler<AddCourseThumbnailCommand, string>
{
    public async Task<string> Handle(AddCourseThumbnailCommand request, CancellationToken cancellationToken)
    {
        //ToDo: add auth
        var course = await courseRepository.GetCourseByIdAsync(request.CourseId);
        var bunny = new BunnyClient(configuration);
        var thumbnailResponse = await bunny.UploadFile(request.File, $"{course.Name}.jpeg", $"CoursesThumbnail");
        if (!thumbnailResponse.IsSuccessful)
        {
            logger.LogWarning("Failed to upload thumbnail image: {ErrorMessage}", thumbnailResponse.Message);
            throw new TryAgain();
        }
        course.ThumbnailUrl = thumbnailResponse.Url;
        await courseRepository.SaveChangesAsync();
        return course.ThumbnailUrl!;
    }
}