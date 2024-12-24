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
}