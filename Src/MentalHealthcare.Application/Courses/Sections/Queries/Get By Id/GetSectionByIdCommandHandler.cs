using AutoMapper;
using MediatR;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Sections.Queries.Get_By_Id;

/// <summary>
/// Handles the retrieval of a specific course section by its ID.
/// </summary>
public class GetSectionByIdCommandHandler(
    ILogger<GetSectionByIdCommandHandler> logger,
    IMapper mapper,
    ICourseSectionRepository sectionRepository
) : IRequestHandler<GetSectionByIdCommand, CourseSectionDto>
{
    public async Task<CourseSectionDto> Handle(GetSectionByIdCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing request to retrieve SectionId: {SectionId} for CourseId: {CourseId}",
            request.SectionId, request.CourseId);

        var courseSection =
            await sectionRepository
                .GetCourseSectionByIdAsync(
                    request.CourseId,
                    request.SectionId
                );

        if (courseSection == null)
        {
            logger.LogWarning("Course section with SectionId: {SectionId} for CourseId: {CourseId} was not found",
                request.SectionId, request.CourseId);
            throw new ResourceNotFound(nameof(courseSection), request.SectionId.ToString());
        }

        var dto = mapper.Map<CourseSectionDto>(courseSection);
        logger.LogInformation("Successfully retrieved SectionId: {SectionId} for CourseId: {CourseId}",
            request.SectionId, request.CourseId);

        return dto;
    }
}