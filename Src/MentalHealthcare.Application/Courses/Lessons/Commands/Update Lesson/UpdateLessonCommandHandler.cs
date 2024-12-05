using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Courses.Lessons.Commands.Add_Lesson;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Lessons.Commands.Update_Lesson;

public class UpdateLessonCommandHandler(
    ILogger<AddLessonCommandHandler> logger,
    ICourseRepository courseRepository
) : IRequestHandler<UpdateLessonCommand, int>
{
    public async Task<int> Handle(UpdateLessonCommand request, CancellationToken cancellationToken)
    {
        var lesson = await courseRepository.GetCourseLessonByIdAsync(request.LessonId);
        if (lesson.CourseId != request.CourseId || lesson.CourseSectionId != request.SectionId)
        {
            logger.LogError($"Course {request.CourseId} doesn't exist");
            throw new ResourceNotFound(nameof(Course), request.CourseId.ToString());
        }

        lesson.LessonName = request.LessonName;
        await courseRepository.SaveChangesAsync();
        return lesson.CourseLessonId;
    }
}