using AutoMapper;
using MediatR;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.LessonResources.Queries.Get_By_id;

public class GetResourceByIdQueryHandler(
    ILogger<GetResourceByIdQueryHandler> logger,
    ICourseResourcesRepository courseResourcesRepository,
    ICourseRepository courseRepository,
    IConfiguration configuration,
    IMapper mapper
    ):IRequestHandler<GetResourceByIdQuery,CourseResourceDto>
{
    public async Task<CourseResourceDto> Handle(GetResourceByIdQuery request, CancellationToken cancellationToken)
    {
       logger.LogInformation("Handling GetResourceByIdQuery");
       var resource =await courseResourcesRepository.GetCourseLessonResourceByIdAsync(request.ResourceId);
       var resourceDto = mapper.Map<CourseResourceDto>(resource);
       return resourceDto;
    }
}