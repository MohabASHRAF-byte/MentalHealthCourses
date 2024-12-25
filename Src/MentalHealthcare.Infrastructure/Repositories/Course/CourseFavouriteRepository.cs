using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Entities.Courses;
using MentalHealthcare.Domain.Repositories.Course;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Repositories.Course;

public class CourseFavouriteRepository(
    MentalHealthDbContext dbContext
) : ICourseFavouriteRepository
{
    public async Task ToggleFavouriteCourseAsync(int courseId, int userId)
    {
        // Check if a favourite course already exists for the given courseId and userId
        var existingFavourite = await dbContext.FavouriteCourses
            .FirstOrDefaultAsync(fc => fc.CourseId == courseId && fc.SystemUserId == userId);

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
                SystemUserId = userId
            };
            await dbContext.FavouriteCourses.AddAsync(newFavourite);
        }

        // Save changes
        await dbContext.SaveChangesAsync();
    }

    public async Task<(int count, List<CourseViewDto> courses)> GetUserFavourites(
        int userId, int pageNumber,
        int pageSize, string searchTerm
    )
    {
        // Start with all favourite courses for the user
        var query = dbContext.FavouriteCourses
            .AsNoTracking()
            .Where(fc => fc.SystemUserId == userId)
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
                IsOwned = false
            })
            //todo: update progress
            .ToListAsync();

        return (totalCount, courses);
    }

    public async Task<(int count, List<SystemUser> users)> GetUsersWhoFavouriteCourseAsync(int courseId, int pageNumber,
        int pageSize, string? searchTerm)
    {
        // Start by querying favourite courses for the given course ID
        var query = dbContext.FavouriteCourses
            .AsNoTracking()
            .Where(fc => fc.CourseId == courseId)
            .Include(fc => fc.SystemUser)
            .AsQueryable();

        // Apply search filter on user's name if a search term is provided
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(fc =>
                (fc.SystemUser.FName + " " + fc.SystemUser.LName)
                .ToLower().Contains(searchTerm.ToLower())
            );
        }

        // Get the total count of users before applying pagination
        var totalCount = await query.CountAsync();

        // Apply pagination and select the users
        var users = await query
            .OrderBy(fc => fc.SystemUser.FName) // Sort by user's first name
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .Select(fc => fc.SystemUser) // Select the user entity
            .ToListAsync();

        return (totalCount, users);
    }
}