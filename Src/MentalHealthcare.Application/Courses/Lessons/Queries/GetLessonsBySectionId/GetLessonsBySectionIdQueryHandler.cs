using AutoMapper;
using MediatR;
using MentalHealthcare.Domain.Repositories.Course;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Lessons.Queries.GetLessonsBySectionId;

public class GetLessonsBySectionIdQueryHandler(
    ILogger<GetLessonsBySectionIdQueryHandler> logger,
    ICourseLessonRepository lessonRepository,
    IMapper mapper,
    IUserContext userContext
) : IRequestHandler<GetLessonsBySectionIdQuery, List<CourseLessonViewDto>>
{
    public async Task<List<CourseLessonViewDto>> Handle(GetLessonsBySectionIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Fetching lessons for course ID {CourseId} and section ID {SectionId}",
            request.CourseId, request.CourseSectionId);

        // Authenticate and validate admin permissions
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        {
            var userDetails = currentUser == null
                ? "User is null"
                : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}";

            logger.LogWarning("Unauthorized access attempt to fetch lessons. User details: {UserDetails}", userDetails);
            throw new ForBidenException("You do not have permission to access this resource.");
        }

        logger.LogInformation("Admin access granted for user {UserId}", currentUser.Id);

        // Retrieve lessons from the repository
        var lessons = await lessonRepository.GetCourseLessonsDto(request.CourseId, request.CourseSectionId);

        if (lessons == null || lessons.Count == 0)
        {
            logger.LogWarning("No lessons found for course ID {CourseId} and section ID {SectionId}.", request.CourseId, request.CourseSectionId);
            throw new ResourceNotFound("course",$"{request.CourseId}.");
        }

        logger.LogInformation("Found {LessonCount} lessons, mapping to DTO", lessons.Count);

        // Map lessons to the view DTO
        var mappedLessons = mapper.Map<List<CourseLessonViewDto>>(lessons);

        logger.LogInformation("Successfully mapped {LessonCount} lessons to DTO", lessons.Count);

        return mappedLessons;
    }
}
