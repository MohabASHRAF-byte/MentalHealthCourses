using AutoMapper;
using MediatR;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Lessons.Queries.GetById;

public class GetLessonByIdQueryHandler(
    ILogger<GetLessonByIdQueryHandler> logger,
    ICourseLessonRepository lessonRepository,
    IMapper mapper
) : IRequestHandler<GetLessonByIdQuery, CourseLessonDto>
{
    public async Task<CourseLessonDto> Handle(GetLessonByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Fetching course lesson with ID {request.LessonId}");

        // Retrieve the full lesson by ID
        var lesson = await lessonRepository.GetCourseFullLessonByIdAsync(request.LessonId);

        // Check if the lesson exists
        if (lesson == null)
        {
            logger.LogWarning($"Lesson with ID {request.LessonId} not found");
            throw new Exception($"Lesson with ID {request.LessonId} not found");
        }

        logger.LogInformation($"Successfully fetched lesson with ID {request.LessonId}, mapping to DTO");

        // Map the lesson entity to a DTO
        var lessonDto = mapper.Map<CourseLessonDto>(lesson);

        logger.LogInformation($"Successfully mapped lesson ID {request.LessonId} to DTO");

        return lessonDto;
    }
}