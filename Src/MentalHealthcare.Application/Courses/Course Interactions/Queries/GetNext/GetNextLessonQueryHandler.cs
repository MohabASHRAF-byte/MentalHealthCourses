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

namespace MentalHealthcare.Application.Courses.Course_Interactions.Queries.GetNext
{
    public class GetNextLessonQueryHandler(

      ILogger<GetNextLessonQueryHandler> logger,
    ICourseInteractionsRepository courseInteractionsRepository,
    IUserContext userContext


        ) : IRequestHandler<GetNextLessonQuery, CourseLessonDto>
    {
        public async Task<CourseLessonDto> Handle(GetNextLessonQuery request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Fetching next lesson for CourseId: {CourseId}", request.CourseId);

            var currentUser = userContext.EnsureAuthorizedUser([UserRoles.User], logger);
 logger.LogInformation("User with SysUserId: {SysUserId} is attempting to fetch LessonId: {LessonId}",
                currentUser.SysUserId, request.LessonId);

            try
            {
                var courseLesson = await courseInteractionsRepository.GetLessonV2(request.CourseId, request.LessonId,
                    (int)currentUser.SysUserId!);

                logger.LogInformation("Successfully fetched LessonId: {LessonId} for UserId: {UserId}",
                    request.LessonId, currentUser.SysUserId);

                return courseLesson;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch LessonId: {LessonId} for UserId: {UserId}",
                    request.LessonId, currentUser.SysUserId);
                throw;
            }
        }



    }
    }

