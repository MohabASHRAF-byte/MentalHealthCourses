using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Dtos.Category;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Dtos.User;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Infrastructure.Repositories.Course;

public class CourseRepository(
    MentalHealthDbContext dbContext,
    ILogger<CourseRepository> logger
) : ICourseRepository
{
    public async Task<int> CreateAsync(Domain.Entities.Course course, List<int> categoryIds)
    {
        try
        {
            var categories = await dbContext.Categories
                .Where(c => categoryIds.Contains(c.CategoryId))
                .ToListAsync();

            course.Categories = categories;

            await dbContext.Courses.AddAsync(course);
            await dbContext.SaveChangesAsync();

            return course.CourseId;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while saving the course.");
            throw;
        }
    }


    public async Task UpdateCourse(Domain.Entities.Course course)
    {
        dbContext.Courses.Update(course);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Domain.Entities.Course> GetFullCourseByIdAsync(
        int id
    )
    {
        var course = await dbContext.Courses
            .AsNoTracking() // Avoid tracking for read-only queries
            .Include(c => c.Instructor)
            .Include(c => c.CourseSections) // Include sections
            .ThenInclude(cs => cs.Lessons) // Include lessons within sections
            .ThenInclude(cl => cl.CourseLessonResources) // Include materials within lessons
            .Include(c => c.Categories) // Include categories
            .FirstOrDefaultAsync(c => c.CourseId == id);

        if (course == null)
        {
            throw new ResourceNotFound(nameof(Domain.Entities.Course), id.ToString());
        }

        return course;
    }

    public async Task<Domain.Entities.Course> GetMinimalCourseByIdAsync(int id)
    {
        var course = await dbContext.Courses
            .FirstOrDefaultAsync(c => c.CourseId == id);
        if (course == null)
        {
            throw new ResourceNotFound("Video", id.ToString());
        }

        return course;
    }


    public async Task<(int TotalCount, IEnumerable<CourseViewDto> Courses)>
        GetAllAsync(
            int? userId,
            string? searchName,
            int requestPageNumber,
            int requestPageSize)
    {
        searchName ??= string.Empty;
        searchName = searchName.ToLower();

        var baseQuery = dbContext.Courses
            .Where(r => r.Name.ToLower().Contains(searchName));

        var totalCount = await baseQuery.CountAsync();

        var courses = await baseQuery
            .Where(c => c.IsArchived == false)
            .Skip(requestPageSize * (requestPageNumber - 1))
            .Take(requestPageSize)
            .Select(c => new CourseViewDto
            {
                CourseId = c.CourseId,
                Name = c.Name,
                ThumbnailUrl = c.ThumbnailUrl,
                Price = c.Price,
                Rating = c.ReviewsCount > 0 ? Math.Round(c.Rating / c.ReviewsCount, 1) : null,
                EnrollmentsCount = c.EnrollmentsCount,
                ReviewsCount = c.ReviewsCount,
                IsOwned =
                    userId != null &&
                    dbContext.CourseProgresses
                        .Any(cp => cp.CourseId == c.CourseId && cp.SystemUserId == userId),
                IsFree = c.IsFree || c.Price == 0,
                Categories = c.Categories == null
                    ? new List<MiniCategoryDto>()
                    : c.Categories.Select(cat => new MiniCategoryDto
                    {
                        CategoryId = cat.CategoryId,
                        Name = cat.Name
                    }).ToList()
            })
            .ToListAsync();

        return (totalCount, courses);
    }


    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }

    public async Task AddPendingUpload(PendingVideoUpload pendingVideoUpload)
    {
        await dbContext.AddAsync(pendingVideoUpload);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeletePending(string requestVideoId)
    {
        var videoUpload = await dbContext.VideoUploads
            .FirstOrDefaultAsync(c => c.PendingVideoUploadId == requestVideoId);
        if (videoUpload == null)
        {
            throw new ResourceNotFound("Video", requestVideoId);
        }

        dbContext.VideoUploads.Remove(videoUpload);
        await dbContext.SaveChangesAsync();
    }

    public async Task<PendingVideoUpload> GetPendingUpload(string requestVideoId)
    {
        var videoUpload = await dbContext.VideoUploads
            .FirstOrDefaultAsync(c => c.PendingVideoUploadId == requestVideoId);
        if (videoUpload == null)
        {
            throw new ResourceNotFound("Video", requestVideoId);
        }

        return videoUpload;
    }

    public async Task<string> GetCourseCollectionId(int courseId)
    {
        var collectionId = await dbContext.Courses
            .Where(c => c.CourseId == courseId)
            .Select(c => c.CollectionId)
            .FirstOrDefaultAsync();
        if (collectionId == null)
        {
            throw new ResourceNotFound("Course", courseId.ToString());
        }

        return collectionId;
    }

    public async Task<string> GetCourseName(int courseId)
    {
        var videoName = await dbContext.Courses
            .Where(c => c.CourseId == courseId)
            .Select(c => c.Name)
            .FirstOrDefaultAsync();
        if (videoName == null)
        {
            throw new ResourceNotFound("Course", courseId.ToString());
        }

        return videoName;
    }

    public async Task<bool> DoesCourseExist(int courseId)
    {
        return await dbContext.Courses.AnyAsync(c => c.CourseId == courseId);
    }
}