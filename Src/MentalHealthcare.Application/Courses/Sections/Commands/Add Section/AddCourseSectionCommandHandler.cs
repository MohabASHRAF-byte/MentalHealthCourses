using AutoMapper;
using MediatR;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Sections.Commands.Add_Section;

public class AddCourseSectionCommandHandler(
    ILogger<AddCourseSectionCommandHandler> logger,
    ICourseRepository courseRepository,
    ICourseSectionRepository sectionRepository,
    IMapper mapper
) : IRequestHandler<AddCourseSectionCommand, int>
{
    public async Task<int> Handle(AddCourseSectionCommand request, CancellationToken cancellationToken)
    {
        //ToDo
        // add Auth
        // add validator
        var newCourseSection = mapper.Map<CourseSection>(request);
        newCourseSection.Name = request.Name;
        await sectionRepository.AddCourseSection(newCourseSection);
        return newCourseSection.CourseSectionId;
    }
}