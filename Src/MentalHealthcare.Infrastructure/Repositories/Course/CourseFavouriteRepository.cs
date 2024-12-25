using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities.Courses;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Repositories.Course;

public class CourseFavouriteRepository(
    MentalHealthDbContext dbContext
) : ICourseFavouriteRepository
{
    public async Task ToggleFavouriteCourseAsync(int courseId, string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");
        }

        // Check if a favourite course already exists for the given courseId and userId
        var existingFavourite = await dbContext.FavouriteCourses
            .FirstOrDefaultAsync(fc => fc.CourseId == courseId && fc.UserId == userId);

        if (existingFavourite != null)
        {
            // If the record exists, remove it
            dbContext.FavouriteCourses.Remove(existingFavourite);
        }
        else
        {
            // Otherwise, add a new favourite record
            var newFavourite = new FavouriteCourse
            {
                CourseId = courseId,
                UserId = userId
            };
            await dbContext.FavouriteCourses.AddAsync(newFavourite);
        }

        // Save changes
        await dbContext.SaveChangesAsync();
    }

    public async Task<(int count, List<CourseViewDto> courses)> GetUserFavourites(
        string userId, int pageNumber,
        int pageSize, string searchTerm
    )
    {
        // Start with all favourite courses for the user
        var query = dbContext.FavouriteCourses
            .AsNoTracking()
            .Where(fc => fc.UserId == userId)
            .Include(fc => fc.Course)
            .AsQueryable();

        // Apply search filter
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query
                .Where(fc => fc.Course.Name
                    .ToLower()
                    .Contains(searchTerm.ToLower())
                );
        }

        // Get the total count before applying pagination
        var totalCount = await query.CountAsync();

        // Apply pagination
        var courses = await query
            .OrderBy(fc => fc.Course.Name) // Sort by course name
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .Select(fc => new CourseViewDto
            {
                Name = fc.Course.Name,
                ThumbnailUrl = fc.Course.ThumbnailUrl,
                Price = fc.Course.Price,
                Rating =
                    (fc.Course.ReviewsCount > 0 ? fc.Course.Rating / fc.Course.ReviewsCount : 0),
                ReviewsCount = fc.Course.ReviewsCount,
                EnrollmentsCount = fc.Course.EnrollmentsCount,
                IsOwned = dbContext.CourseProgresses.Any(cp =>
                    cp.UserId == userId && cp.CourseId == fc.Course.CourseId) 
            })
            .ToListAsync();

        return (totalCount, courses);
    }
}