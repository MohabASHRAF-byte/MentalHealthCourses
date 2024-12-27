using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Dtos.User;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories.Course;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Repositories.Course;

public class CourseReview(
    MentalHealthDbContext dbContext
) : ICourseReview
{
    public async Task AddCourseReviewAsync(UserReview review)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var course = await dbContext.Courses
                .Where(c => c.CourseId == review.courseId)
                .FirstOrDefaultAsync();

            if (course == null)
            {
                throw new ArgumentException("Course does not exist");
            }

            var totalReviewsMadeWithUser = await dbContext.UserReviews
                .Where(ur => ur.SystemUserId == review.SystemUserId)
                .CountAsync();
            if (totalReviewsMadeWithUser >= Global.UserReviewsLimit)
            {
                throw new ArgumentException("Reviews limit reached Delete Some Before Add new ");
            }

            var totalLessons = await dbContext.CourseSections
                .Where(cs => cs.CourseId == review.courseId)
                .SelectMany(cs => cs.Lessons)
                .CountAsync();

            var lastCompletedLessonIndex = await dbContext.CourseProgresses
                .Where(cp => cp.CourseId == review.courseId && cp.SystemUserId == review.SystemUserId)
                .Select(cp => cp.LastLessonIdx)
                .FirstOrDefaultAsync();

            if (lastCompletedLessonIndex < Math.Ceiling(totalLessons * Global.CourseCompleteToReview))
            {
                throw new ArgumentException(
                    $"You must complete at least {Global.CourseCompleteToReview}% of the course to leave a review.");
            }

            // Update course ratings and reviews count atomically
            course.ReviewsCount++;
            course.Rating += (decimal)review.Rating;

            await dbContext.UserReviews.AddAsync(review);
            dbContext.Courses.Update(course);

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<(int count, IEnumerable<UserReviewDto> reviews)> GetCoursesReviewsAsync(
        int courseId,
        int pageNumber,
        int pageSize,
        int contentLimit // the max number of char in the review
        )
    {
        var reviewsQuery = dbContext.UserReviews
            .Where(r => r.courseId == courseId)
            .Include(r => r.User)
            .AsQueryable();

        var totalCount = await reviewsQuery.CountAsync();

        var reviews = await reviewsQuery
            .OrderByDescending(r => r.ReviewDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new UserReviewDto
            {
                UserReviewId = r.UserReviewId,
                Content = r.Content.Length > contentLimit
                    ? r.Content.Substring(0, contentLimit) + "..."
                    : r.Content, // Trim content if it exceeds limit
                Rating = r.Rating,
                Seconds = (long)(DateTime.UtcNow - r.ReviewDate).TotalSeconds,
                SystemUserId = r.SystemUserId,
                user = new SystemUserDto
                {
                    Username = r.User.User.UserName!,
                    Name = r.User.FName + " " + r.User.LName,
                }
            })
            .ToListAsync();

        return (totalCount, reviews);
    }
}