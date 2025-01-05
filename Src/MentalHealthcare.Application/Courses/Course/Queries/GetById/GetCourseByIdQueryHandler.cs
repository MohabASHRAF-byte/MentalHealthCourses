using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Courses.Course.Commands.Create;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Course.Queries.GetById;

public class GetCourseByIdQueryHandler(
    ILogger<CreateCourseCommandHandler> logger,
    IMapper mapper,
    ICourseRepository courseRepository,
    ICourseReview courseReview,
    IUserContext userContext,
    ICourseFavouriteRepository courseFavouriteRepository,
    ICourseInteractionsRepository courseInteractionsRepository
) : IRequestHandler<GetCourseByIdQuery, CourseDto>
{
    public async Task<CourseDto> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting Handle method for GetCourseByIdQuery with CourseId: {CourseId}", request.Id);

        // Retrieve the current user
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null)
        {
            logger.LogWarning(
                "Unauthorized access attempt to retrieve course details. User information: {UserDetails}",
                currentUser == null
                    ? "User is null"
                    : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}"
            );
            throw new ForBidenException("You do not have permission to view this course.");
        }

        logger.LogInformation("Retrieving course details for CourseId: {CourseId}", request.Id);
        var course = await courseRepository.GetFullCourseByIdAsync(request.Id);

        if (course == null)
        {
            logger.LogWarning("Course with ID {CourseId} not found.", request.Id);
            throw new ResourceNotFound(nameof(course), request.Id.ToString());
        }

        if (course.IsArchived && currentUser.HasRole(UserRoles.User))
        {
            logger.LogInformation("Course with ID {CourseId} is archived.", request.Id);
            throw new ResourceNotFound(nameof(course), request.Id.ToString());
        }

        logger.LogInformation("Mapping course entity to CourseDto.");
        var courseDto = mapper.Map<CourseDto>(course);
        courseDto.Rating ??= 0;

        if (course.ReviewsCount != 0 && course.Rating != 0)
        {
            courseDto.Rating = Math.Round(course.Rating / course.ReviewsCount, 1);
        }

        courseDto.CreatedAt = course.CreatedAt;
        courseDto.SecondsSinceCreation = (long)DateTime.UtcNow.Subtract(course.CreatedAt).TotalSeconds;

        logger.LogInformation("Fetching reviews for CourseId: {CourseId}.", request.Id);
        var (count, reviews) = await courseReview
            .GetCoursesReviewsAsync(
                request.Id, 1, 5, 50
            );
        courseDto.UserReviews = reviews;
        courseDto.ReviewsCount = count;

        if (currentUser.HasRole(UserRoles.User))
        {
            logger.LogInformation("Checking if CourseId: {CourseId} is a favourite or owned by the user.", request.Id);
            courseDto.IsFavourite =
                await courseFavouriteRepository.HasFavouriteCourseAsync(request.Id, currentUser.SysUserId!.Value);
            courseDto.IsOwned =
                await courseInteractionsRepository.IsCourseOwner(request.Id, currentUser.SysUserId!.Value);
            foreach (var section in courseDto.CourseSections)
            {
                foreach (var lesson in section.Lessons)
                {
                    lesson.Url = "";
                    foreach (var resource in lesson.CourseLessonResources)
                    {
                        resource.Url = "";
                    }
                }
            }
        }
        else
        {
            logger.LogInformation("User is admin, setting IsFavourite and IsOwned to null.");
            courseDto.IsOwned = null;
            courseDto.IsFavourite = null;
        }


        logger.LogInformation("Successfully retrieved course details for CourseId: {CourseId}.", request.Id);
        return courseDto;
    }
}