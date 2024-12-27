using MentalHealthcare.Domain.Constants;
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
}