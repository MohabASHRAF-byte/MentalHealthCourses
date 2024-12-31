using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Dtos.Category;
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
    public async Task<int> CreateAsync(Domain.Entities.Courses.Course course, List<int> categoryIds)
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

    public async Task<int> UpdateCourseAsync(
        int courseId,
        string? name,
        decimal? price,
        string? description,
        int? instructorId,
        List<int>? categoryId,
        bool? isFree,
        bool? isFeatured,
        bool? isArchived
    )
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            // Retrieve course from the database
            var course = await dbContext.Courses
                .Where(c => c.CourseId == courseId)
                .Include(c => c.Categories)
                .FirstOrDefaultAsync();
            if (course == null)
            {
                throw new ResourceNotFound(nameof(course), courseId.ToString());
            }

            if (isFree.HasValue)
            {
                course.IsFree = isFree.Value;
            }

            if (isFeatured.HasValue)
            {
                course.IsFeatured = isFeatured.Value;
            }

            if (isArchived.HasValue)
            {
                course.IsArchived = isArchived.Value;
            }

            // Update course properties if values are provided
            if (!string.IsNullOrWhiteSpace(name))
            {
                course.Name = name;
            }

            if (!string.IsNullOrWhiteSpace(description))
            {
                course.Description = description;
            }

            if (price.HasValue)
            {
                course.Price = price.Value;
            }

            if (instructorId.HasValue)
            {
                var instructor = await dbContext.Instructors.FindAsync(instructorId.Value);
                if (instructor == null)
                {
                    throw new ResourceNotFound(nameof(instructor), instructorId.ToString() ?? "");
                }

                course.Instructor = instructor;
            }

            if (categoryId != null)
            {
                course.Categories?.Clear();
                var categories = await dbContext.Categories
                    .Where(c => categoryId.Contains(c.CategoryId))
                    .ToListAsync();

                course.Categories = categories;
            }

            // Save changes to the database
            dbContext.Courses.Update(course);
            await dbContext.SaveChangesAsync();

            // Commit the transaction
            await transaction.CommitAsync();

            return course.CourseId;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex, "An error occurred while updating the course with ID {CourseId}", courseId);
            throw;
        }
    }


    public async Task UpdateCourse(Domain.Entities.Courses.Course course)
    {
        dbContext.Courses.Update(course);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Domain.Entities.Courses.Course> GetFullCourseByIdAsync(
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
            throw new ResourceNotFound(nameof(Domain.Entities.Courses.Course), id.ToString());
        }

        return course;
    }

    public async Task<Domain.Entities.Courses.Course> GetMinimalCourseByIdAsync(int id)
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
                IconUrl = c.IconUrl,
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

    public async Task<bool> UpdateCourseProgressAfterDeletingSection(
        int courseId,
        int sectionId,
        int sectionStart,
        int sectionEnd
    )
    {
        // Retrieve the order of the deleted section and its size
        var oldSectionOrder = await dbContext.CourseSections
            .Where(cs => cs.CourseId == courseId && cs.CourseSectionId == sectionId)
            .Select(cs => cs.Order)
            .FirstOrDefaultAsync();

        if (oldSectionOrder == 0)
        {
            throw new ResourceNotFound("Section", sectionId.ToString());
        }

        // Find the last lesson index of the previous section
        var previousSectionLastLessonIdx = await dbContext.CourseSections
            .Where(cs => cs.CourseId == courseId && cs.Order == oldSectionOrder - 1)
            .SelectMany(cs => cs.Lessons)
            .OrderByDescending(cl => cl.OrderOnCourse)
            .Select(cl => cl.OrderOnCourse)
            .FirstOrDefaultAsync();

        // Default to 0 if no previous section exists
        previousSectionLastLessonIdx =
            previousSectionLastLessonIdx > 0 ? previousSectionLastLessonIdx : 0;

        // Fetch all progresses for the course
        var progresses = await dbContext.CourseProgresses
            .Where(cp => cp.CourseId == courseId)
            .ToListAsync();

        // Preload section order mappings for all lessons
        var lessonSectionOrders = await dbContext.CourseLessons
            .Where(cl => cl.CourseSection.CourseId == courseId)
            .Select(cl => new { cl.OrderOnCourse, SectionOrder = cl.CourseSection.Order })
            .ToDictionaryAsync(cl => cl.OrderOnCourse, cl => cl.SectionOrder);

        foreach (var progress in progresses)
        {
            if (!lessonSectionOrders.TryGetValue(progress.LastLessonIdx, out var sectionOrder))
            {
                continue; // Skip if progress index is invalid
            }

            if (sectionOrder == oldSectionOrder)
            {
                // If progress is in the deleted section
                progress.LastLessonIdx = previousSectionLastLessonIdx;
            }
            else if (sectionOrder > oldSectionOrder)
            {
                // If progress is in a section after the deleted section
                progress.LastLessonIdx -= (sectionEnd - sectionStart + 1);
            }
            // Do nothing for sections before the deleted section
        }

        // Save updated progress
        dbContext.CourseProgresses.UpdateRange(progresses);
        await dbContext.SaveChangesAsync();

        return true;
    }
    public async Task BeginTransactionAsync()
    {
        await dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await dbContext.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await dbContext.Database.RollbackTransactionAsync();
    }
}