using System.Text.Json.Serialization;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Repositories.Course;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Sections.Queries.Get_All;

/// <summary>
/// Handles the retrieval of all course sections for a specific course.
/// </summary>
public class GetAllCourseSectionsQueryHandler(
    ILogger<GetAllCourseSectionsQueryHandler> logger,
    ICourseSectionRepository sectionRepository,
    IUserContext userContext
) : IRequestHandler<GetAllCourseSectionsQuery, IEnumerable<CourseSectionViewDto>>
{
    public async Task<IEnumerable<CourseSectionViewDto>> Handle(GetAllCourseSectionsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetAllCourseSectionsQuery for CourseId: {CourseId}", request.courseId);

        // Authenticate and validate user permissions
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        {
            var userDetails = currentUser == null
                ? "User is null"
                : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}";

            logger.LogWarning("Unauthorized access attempt to retrieve course sections. User details: {UserDetails}",
                userDetails);
            throw new ForBidenException("You do not have permission to view course sections.");
        }

        // Retrieve course sections
        var courseSectionDtos =
            await sectionRepository
                .GetCourseSectionsByCourseIdAsync(request.courseId);

        var courseSectionViewDtos = courseSectionDtos.ToList();
        logger.LogInformation("Retrieved {TotalCount} course sections for CourseId: {CourseId}",
            courseSectionViewDtos.Count(), request.courseId);

        // Return the result
        return courseSectionViewDtos;
    }
}