using AutoMapper;
using MediatR;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Lessons.Commands.Update_Lesson;

public class UpdateLessonCommandHandler(
    ILogger<UpdateLessonCommandHandler> logger,
    ICourseRepository courseRepository,
    ICourseLessonRepository courseLessonRepository
) : IRequestHandler<UpdateLessonCommand, int>
{
    public async Task<int> Handle(UpdateLessonCommand request, CancellationToken cancellationToken)
    {
        await courseLessonRepository.UpdateCourseLessonDataAsync(request.LessonId, request.LessonName);
        return request.LessonId;
    }
}