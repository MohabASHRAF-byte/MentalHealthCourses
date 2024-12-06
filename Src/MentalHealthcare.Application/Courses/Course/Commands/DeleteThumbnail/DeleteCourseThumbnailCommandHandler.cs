using MediatR;
using MentalHealthcare.Application.BunnyServices.Files.DeleteFile;
using MentalHealthcare.Application.Courses.Commands.DeleteThumbnail;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories.Course;

namespace MentalHealthcare.Application.Courses.Course.Commands.DeleteThumbnail;

public class DeleteCourseThumbnailCommandHandler(
    ICourseRepository courseRepository,
    IMediator mediator
) : IRequestHandler<DeleteCourseThumbnailCommand>
{
    public async Task Handle(DeleteCourseThumbnailCommand request, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetMinimalCourseByIdAsync(request.CourseId);
        if (course.ThumbnailName is null)
        {
            return;
        }
        var command = new DeleteFileCommand()
        {
            FileName = course.ThumbnailName,
            Directory = Global.CourseThumbnailDirectory
        };
        
        await mediator.Send(command, cancellationToken);
        course.ThumbnailUrl = null;
        await courseRepository.SaveChangesAsync();
    }
}