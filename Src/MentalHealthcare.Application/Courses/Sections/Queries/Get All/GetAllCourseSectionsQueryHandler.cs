using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Sections.Queries.Get_All;

/// <summary>
/// Handles the retrieval of course sections with pagination and search functionality.
/// </summary>
public class GetAllCourseSectionsQueryHandler(
    ILogger<GetAllCourseSectionsQueryHandler> logger,
    IMapper mapper,
    ICourseSectionRepository sectionRepository
) : IRequestHandler<GetAllCourseSectionsQuery, PageResult<CourseSectionViewDto>>
{
    public async Task<PageResult<CourseSectionViewDto>> Handle(GetAllCourseSectionsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetAllCourseSectionsQuery for CourseId: {CourseId}, Page: {PageNumber}, Size: {PageSize}, Search: {SearchString}", 
            request.courseId, request.PageNumber, request.PageSize, request.SearchString);

        // Retrieve course sections with pagination and search
        (int totalCount, IEnumerable<CourseSectionViewDto> courseSectionDtos) =
            await sectionRepository.GetCourseSectionsByCourseIdAsync(
                courseId: request.courseId,
                search: request.SearchString,
                requestPageSize: request.PageSize,
                requestPageNumber: request.PageNumber
            );

        logger.LogInformation("Retrieved {TotalCount} course sections for CourseId: {CourseId}", totalCount, request.courseId);

        // Construct and return paginated result
        return new PageResult<CourseSectionViewDto>(
            courseSectionDtos,
            totalCount,
            request.PageSize,
            request.PageNumber
        );
    }
}