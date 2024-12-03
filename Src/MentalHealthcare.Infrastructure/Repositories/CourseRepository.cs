using MentalHealthcare.Application.Courses;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace MentalHealthcare.Infrastructure.Repositories;

public class CourseRepository(
    MentalHealthDbContext dbContext,
    ILogger<CourseRepository> logger
) : ICourseRepository
{
    public async Task<int> CreateAsync(Course course)
    {
        try
        {
            await dbContext.Courses.AddAsync(course);
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException is PostgresException postgresException)
            {
                // Check for the specific foreign key constraint violation
                if (postgresException.SqlState == PostgresConstants.ForeignKeyViolation &&
                    postgresException.Message.Contains(PostgresConstants.CourseInstructorFkConstrain))
                {
                    logger.LogError(ex, "Foreign key violation: {Message}", postgresException.Message);
                    throw new ResourceNotFound(nameof(Instructor), course.InstructorId.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while saving the course.");
            throw;
        }

        return course.CourseId;
    }

    public async Task UpdateCourse(Course course)
    {
         dbContext.Courses.Update(course);
        await dbContext.SaveChangesAsync();
    }

    public async Task<CourseDto> GetByIdAsync(int id)
    {
        var course = await dbContext.Courses
            .AsNoTracking()
            .Where(c => c.CourseId == id)
            .Select(c => new CourseDto
            {
                CourseId = c.CourseId,
                Name = c.Name,
                ThumbnailUrl = c.ThumbnailUrl,
                Price = c.Price,
                Rating = c.Rating,
                ReviewsCount = c.ReviewsCount,
                EnrollmentsCount = c.EnrollmentsCount,
                Description = c.Description,
                IsFree = c.IsFree,
                IsPublic = c.IsPublic,
                favouritesCount = c.UsersFavCourse.Count(),
                Instructor = new InstructorDto
                {
                    InstructorId = c.Instructor.InstructorId,
                    Name = c.Instructor.Name
                },
                CourseMateriels = c.CourseMateriels
                    .Select(cm => new CourseMaterielDto
                    {
                        ItemOrder = cm.ItemOrder,
                        Description = cm.Description,
                        Title = cm.Title,
                        Url = cm.Url,
                        IsVideo = cm.IsVideo,
                        CourseMaterielId = cm.CourseMaterielId,
                    }).ToList(),
                Categories = c.Categories.Select(cat => new CategoryDto
                {
                    CategoryId = cat.CategoryId,
                    Name = cat.Name
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (course == null)
        {
            throw new ResourceNotFound(nameof(Course), id.ToString());
        }

        return course;
    }

    public async Task<Course> GetCourseByIdAsync(int id)
    {
        var course = await dbContext.Courses
            .Include(c => c.CourseMateriels)
            .FirstOrDefaultAsync(c => c.CourseId == id);
        if (course == null)
        {
            throw new ResourceNotFound(nameof(Course), id.ToString());
        }

        return course;
    }


    public async Task<(int, IEnumerable<Course>)> GetAllAsync(string? searchName, int requestPageNumber,
        int requestPageSize)
    {
        searchName ??= string.Empty;
        searchName = searchName.ToLower();
        var baseQuery = dbContext.Courses
            .Where(r => r.Name.ToLower().Contains(searchName));
        var totalCount = await baseQuery.CountAsync();
        var courses = await baseQuery
            .Skip(requestPageSize * (requestPageNumber - 1))
            .Take(requestPageSize)
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

    public int GetVideoOrder(int pendingCourseId)
    {
        var maxValue = dbContext.CourseMateriels
                .Where(c => c.CourseId == pendingCourseId)
                .Select(c => (int?)c.ItemOrder).ToList() // Use nullable to handle empty result sets
            ;
        var ret = maxValue.Max();

        return ret.HasValue ? ret.Value : 1; // Return 1 if maxValue is null
    }


    public async Task AddCourseMatrial(CourseMateriel courseMateriel)
    {
        await dbContext.CourseMateriels.AddAsync(courseMateriel);
        await dbContext.SaveChangesAsync();
    }

    public async Task<int> AddCourseSection(CourseSection courseSection)
    {
        var maxOrder = await dbContext.CourseSections
            .Where(c => c.CourseId == courseSection.CourseId)
            .MaxAsync(c => (int?)c.Order) ?? 0;

        courseSection.Order = maxOrder + 1;

        await dbContext.CourseSections.AddAsync(courseSection);
        return await dbContext.SaveChangesAsync();
    }

    public async Task<List<CourseSection>> GetCourseSections(int courseId)
    {
        var sections = await dbContext.CourseSections.Where(c => c.CourseId == courseId).ToListAsync();
        return sections;
    }



    public async Task UpdateCourseSectionsAsync(List<CourseSection> courseSection)
    {
        dbContext.CourseSections.UpdateRange(courseSection);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteCourseSectionAsync(CourseSection courseSection)
    {
        dbContext.CourseSections.Remove(courseSection);
        await dbContext.SaveChangesAsync();
    }
}