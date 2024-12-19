using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Courses.Commands.Create;
using MentalHealthcare.Application.Courses.Course.Queries.GetAll;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Queries.GetAll;

/// <summary>
/// Handles the retrieval of all courses with optional search criteria.
/// </summary>
public class GetAllCoursesQueryHandler(
    ILogger<GetAllCoursesQueryHandler> logger,
    IMapper mapper,
    ICourseRepository courseRepository
    ): IRequestHandler<GetAllCoursesQuery, PageResult<CourseViewDto>>
{
    /// <summary>
    /// Processes the query to retrieve all courses based on the provided search criteria and pagination details.
    /// </summary>
    /// <param name="request">The query containing the search text, page number, and page size.</param>
    /// <param name="cancellationToken">Token to observe for cancellation requests.</param>
    /// <returns>A task that represents the result of the operation, containing a paginated list of course view DTOs.</returns>
    /// 
    /// <remarks>
    /// The following describes the logic flow of the method:
    /// <list type="number">
    /// <item>
    /// <description>Log the start of the retrieval process for all courses.</description>
    /// </item>
    /// <item>
    /// <description>Retrieve all courses using the course repository:
    /// <list type="bullet">
    /// <item>Pass the search text, page number, and page size to <c>GetAllAsync</c>.</item>
    /// <item>Await the result containing the list of courses and the total count.</item>
    /// </list>
    /// </description>
    /// </item>
    /// <item>
    /// <description>Map the retrieved courses to DTOs:
    /// <list type="bullet">
    /// <item>Use <c>mapper.Map&lt;IEnumerable&lt;CourseViewDto&gt;&gt;(pendingUsers.Item2)</c> to convert the course entities.</item>
    /// </list>
    /// </description>
    /// </item>
    /// <item>
    /// <description>Create a <see cref="PageResult&lt;CourseViewDto&gt;"/> containing the mapped DTOs and the total count.</description>
    /// </item>
    /// </list>
    /// </remarks>
    public async Task<PageResult<CourseViewDto>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        // TODO: add auth
        logger.LogInformation("Retrieving all courses with search text: {SearchText}, page number: {PageNumber}, page size: {PageSize}", request.SearchText, request.PageNumber, request.PageSize);
        
        // Retrieve all courses from the repository
        var pendingUsers = await courseRepository.GetAllAsync(request.SearchText, request.PageNumber, request.PageSize);
        
        // Log the number of courses retrieved
        logger.LogInformation("Retrieved {Count} courses.", pendingUsers.Item1);
        
        // Map the retrieved courses to DTOs
        var usersDtos = mapper.Map<IEnumerable<CourseViewDto>>(pendingUsers.Item2);
        
        // Create the page result
        var count = pendingUsers.Item1;
        var ret = new PageResult<CourseViewDto>(usersDtos, count, request.PageSize, request.PageNumber);
        
        return ret;
    }
}
