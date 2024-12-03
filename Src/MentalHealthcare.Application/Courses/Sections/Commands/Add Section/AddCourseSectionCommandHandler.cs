using MediatR;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Sections.Commands.Add_Section;

public class AddCourseSectionCommandHandler(
    ILogger<AddCourseSectionCommandHandler> logger,
    ICourseRepository courseRepository
) : IRequestHandler<AddCourseSectionCommand, int>
{
    public async Task<int> Handle(AddCourseSectionCommand request, CancellationToken cancellationToken)
    {
        //ToDo
        // add Auth
        // add validator
        var course = await courseRepository.GetCourseByIdAsync(request.CourseId);
        var newCourseSection = new CourseSection
        {
            Course = course,
            CourseId = course.CourseId,
            Name = request.SectionName
        };
        await courseRepository.AddCourseSection(newCourseSection);
        course.CourseSections.Add(newCourseSection);
        await courseRepository.UpdateCourse(course);
        return newCourseSection.CourseSectionId;
    }
}