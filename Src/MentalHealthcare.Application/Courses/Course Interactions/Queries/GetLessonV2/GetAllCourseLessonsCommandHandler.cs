using AutoMapper;
using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Courses.Course_Interactions.Queries.GetLessonV2
{
    public class GetAllCourseLessonsCommandHandler(
        ILogger<GetAllCourseLessonsCommandHandler> logger,
         ICourseInteractionsRepository interactionsRepository,
        IMapper mapper,
        IUserContext userContext) : IRequestHandler<GetAllCourseLessonsCommand, List<CourseLessonDto>>
    {
        public async Task<List<CourseLessonDto>> Handle(GetAllCourseLessonsCommand request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Starting Handle method for GetAllCourseLessonsCommand with CourseId: {CourseId}",
           request.CourseId);

            var currentUser = userContext.EnsureAuthorizedUser([UserRoles.User], logger);

            logger.LogInformation("User with SysUserId: {SysUserId} is attempting to fetch lessons for CourseId: {CourseId}",
                currentUser.SysUserId, request.CourseId);

            if (!int.TryParse(currentUser.SysUserId?.ToString(), out int userId))
            {
                var errorMessage = $"Invalid SysUserId format: {currentUser.SysUserId}";
                logger.LogError(errorMessage);
                throw new UnauthorizedAccessException(errorMessage);
            }

            try
            {
                var lessons = await interactionsRepository.GetAllLessonsForEnrolledUserAsync(request.CourseId, userId);

                logger.LogInformation("Successfully fetched {Count} lessons for CourseId: {CourseId} and UserId: {UserId}",
                    lessons.Count(), request.CourseId, userId);

                return lessons.ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch lessons for CourseId: {CourseId} and UserId: {UserId}",
                    request.CourseId, userId);
                throw;
            }
        }
    }
}
