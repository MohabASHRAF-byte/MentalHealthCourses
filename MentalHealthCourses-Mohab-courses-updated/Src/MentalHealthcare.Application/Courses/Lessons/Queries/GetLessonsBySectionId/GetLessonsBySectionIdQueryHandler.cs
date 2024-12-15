using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Courses.Lessons.Commands.Update_order;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Lessons.Queries.GetLessonsBySectionId;

public class GetLessonsBySectionIdQueryHandler(
    ILogger<GetLessonsBySectionIdQueryHandler> logger,
    ICourseLessonRepository lessonRepository,
    IMapper mapper
) : IRequestHandler<GetLessonsBySectionIdQuery, List<CourseLessonViewDto>>
{
    public async Task<List<CourseLessonViewDto>> Handle(GetLessonsBySectionIdQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Fetching lessons for course ID {request.CourseId} and section ID {request.CourseSectionId}");

        // Retrieve lessons from the repository
        var lessons = await lessonRepository.GetCourseLessonsDto(request.CourseId, request.CourseSectionId);

        logger.LogInformation($"Found {lessons.Count} lessons, mapping to DTO");

        // Map lessons to the view DTO
        var mappedLessons = mapper.Map<List<CourseLessonViewDto>>(lessons);

        logger.LogInformation($"Successfully mapped {lessons.Count} lessons to DTO");

        return mappedLessons;
    }
}