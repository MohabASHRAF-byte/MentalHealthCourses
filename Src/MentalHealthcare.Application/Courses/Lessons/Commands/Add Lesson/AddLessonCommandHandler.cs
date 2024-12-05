using AutoMapper;
using MediatR;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Lessons.Commands.Add_Lesson;

public class AddLessonCommandHandler(
    ILogger<AddLessonCommandHandler> logger,
    ICourseRepository courseRepository,
    IMapper mapper
) : IRequestHandler<AddLessonCommand, int>
{
    public async Task<int> Handle(AddLessonCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Adding new Lesson : {request.LessonName}");
        var section = await courseRepository.GetCourseSectionByIdAsync(request.SectionId);
        if (section.CourseId != request.CourseId)
        {
            logger.LogError($"Course {request.CourseId} doesn't exist");
            throw new ResourceNotFound(nameof(Course), request.CourseId.ToString());
        }

        var lesson = new CourseLesson()
        {
            CourseId = section.CourseId,
            Course = section.Course,
            CourseSectionId = section.CourseSectionId,
            CourseSection = section,
            LessonName = request.LessonName,
        };
        await courseRepository.AddCourseLesson(lesson);
        return lesson.CourseLessonId;
    }
}