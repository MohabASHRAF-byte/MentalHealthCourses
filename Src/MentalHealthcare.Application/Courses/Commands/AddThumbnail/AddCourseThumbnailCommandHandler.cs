using MediatR;
using MentalHealthcare.Application.BunnyServices.Files.UploadFile;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Commands.AddThumbnail;

public class AddCourseThumbnailCommandHandler(
    ILogger<AddCourseThumbnailCommandHandler> logger,
    ICourseRepository courseRepository,
    IMediator mediator
) : IRequestHandler<AddCourseThumbnailCommand, string>
{
    public async Task<string> Handle(AddCourseThumbnailCommand request, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetCourseByIdAsync(request.CourseId);
        var thumbnailName = request.CourseId +"-"+ Guid.NewGuid() + Global.ThumbnailFileExtension;
        var uploadFileCommand = new UploadFileCommand
        {
            File = request.File,
            FileName = thumbnailName,
            Directory = Global.CourseThumbnailDirectory
        };
        var result = await mediator.Send(uploadFileCommand, cancellationToken);
        course.ThumbnailUrl = result;
        course.ThumbnailName = thumbnailName;
        await courseRepository.SaveChangesAsync();
        return result;
    }
}