using MediatR;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.LessonResources.Commands.Update_Resource;

public class UpdateLessonResourceCommandHandler(
    ILogger<UpdateLessonResourceCommandHandler> logger,
    ICourseResourcesRepository courseResourcesRepository,
    IConfiguration configuration
    ): IRequestHandler<UpdateLessonResourceCommand,int>
{
    public async Task<int> Handle(UpdateLessonResourceCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Updated resource for lesson {request.LessonId}.");
        var resource = await courseResourcesRepository.GetCourseLessonResourceByIdAsync(request.ResourceId);
        resource.Title = request.ResourceName;
        await courseResourcesRepository.SaveChangesAsync();
        return resource.CourseLessonResourceId;
    }
}