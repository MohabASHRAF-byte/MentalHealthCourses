using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Dtos.User;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
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

            var totalLessons = course.LessonsCount;

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
                IsFullContent = r.Content.Length <= contentLimit,
                IsEdited = r.IsEdited,
                Rating = r.Rating,
                SecondsSinceCreated = (long)(DateTime.UtcNow - r.ReviewDate).TotalSeconds,
                SecondsSinceLastEdited = (long)(DateTime.UtcNow - r.LastModifiedDate).TotalSeconds,
                SystemUserId = r.SystemUserId,
                courseId = r.courseId,
                user = new SystemUserDto
                {
                    Username = r.User.User.UserName!,
                    Name = r.User.FName + " " + r.User.LName,
                }
            })
            .ToListAsync();

        return (totalCount, reviews);
    }

    public async Task UpdateCourseReviewAsync(
        int userId,
        int courseId,
        int reviewId,
        float? rating,
        string? content)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var review = await dbContext.UserReviews
                .Where(r =>
                    r.SystemUserId == userId &&
                    r.courseId == courseId &&
                    r.UserReviewId == reviewId
                ).FirstOrDefaultAsync();
            if (review == null)
            {
                throw new ResourceNotFound(nameof(Domain.Entities.Courses.Course), nameof(courseId));
            }

            if (!string.IsNullOrWhiteSpace(content))
                review.Content = content;

            if (rating.HasValue)
            {
                var course = await dbContext.Courses.FindAsync(courseId);
                if (course == null)
                    throw new ResourceNotFound(nameof(Domain.Entities.Courses.Course), nameof(courseId));

                course.Rating -= (decimal)review.Rating;
                review.Rating = (float)rating;
                course.Rating += (decimal)review.Rating;
            }

            review.IsEdited = true;
            review.LastModifiedDate = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteCourseReviewAsync(
        int? userId,
        int courseId,
        int reviewId
    )
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var review = await dbContext.UserReviews
                .Where(
                    review =>
                        review.courseId == courseId
                        && review.UserReviewId == reviewId
                ).FirstOrDefaultAsync();
            if (review == null)
            {
                throw new ResourceNotFound(nameof(UserReview), nameof(reviewId));
            }

            if (userId.HasValue && review.SystemUserId != userId.Value)
            {
                throw new ForBidenException("You are not allowed to delete this review.");
            }

            var course = await dbContext.Courses.FindAsync(courseId);
            if (course == null)
            {
                throw new ResourceNotFound(nameof(Domain.Entities.Courses.Course), nameof(courseId));
            }

            course.Rating -= (decimal)review.Rating;
            course.ReviewsCount -= 1;

            dbContext.UserReviews.Remove(review);
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<UserReviewDto> GetUserReviewAsync(
        int courseId,
        int reviewId
    )
    {
        var review = await dbContext.UserReviews
            .Where(r =>
                r.courseId == courseId
                && r.UserReviewId == reviewId
            )
            .Select(r => new UserReviewDto
            {
                UserReviewId = r.UserReviewId,
                Content = r.Content,
                Rating = r.Rating,
                SystemUserId = r.SystemUserId,
                SecondsSinceCreated = (long)(DateTime.UtcNow - r.ReviewDate).TotalSeconds,
                SecondsSinceLastEdited = (long)(DateTime.UtcNow - r.LastModifiedDate).TotalSeconds,
                IsEdited = r.IsEdited,
                IsFullContent = true,
                user = new SystemUserDto
                {
                    Username = r.User.User.UserName!,
                    Name = r.User.FName + " " + r.User.LName
                }
            })
            .FirstOrDefaultAsync();

        if (review == null)
        {
            throw new ResourceNotFound(nameof(UserReview), nameof(reviewId));
        }

        return review;
    }
}