//using AutoMapper;
//using MediatR;
//using MentalHealthcare.Application.SystemUsers;
//using MentalHealthcare.Domain.Constants;
//using MentalHealthcare.Domain.Dtos.course;
//using MentalHealthcare.Domain.Repositories.Course;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MentalHealthcare.Application.Courses.Lessons.Queries.GetAll
//{
//    public class GetAllLessonsInCourseQueryHandler(
//       ILogger<GetAllLessonsInCourseQueryHandler> logger,
//        ICourseLessonRepository lessonRepository,
//        IMapper mapper,
//        IUserContext userContext


//        ) : IRequestHandler<GetAllLessonsInCourseQuery, List<CourseLessonViewDto>>
//    {
//        public async Task<List<CourseLessonViewDto>> Handle(GetAllLessonsInCourseQuery request, CancellationToken cancellationToken)
//        {



//            logger.LogInformation("Fetching all lessons for course ID {CourseId}", request.CourseId);

//            var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin, UserRoles.User], logger);
//            logger.LogInformation("User {UserId} authorized to fetch course lessons.", currentUser.Id);

//            var lessonDtos = await lessonRepository.GetAllLessonsInCourseAsync(request.CourseId);

//            logger.LogInformation("Successfully retrieved {Count} lessons for course ID {CourseId}", lessonDtos.Count, request.CourseId);

//            return lessonDtos;
//        }
//    }
//}
