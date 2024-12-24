using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
namespace MentalHealthcare.Infrastructure.Repositories.Course;

public class CourseRepository(
    MentalHealthDbContext dbContext,
    ILogger<CourseRepository> logger
) : ICourseRepository
{
    public async Task<int> CreateAsync(Domain.Entities.Course course)
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

    public async Task UpdateCourse(Domain.Entities.Course course)
    {
        dbContext.Courses.Update(course);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Domain.Entities.Course> GetFullCourseByIdAsync(int id)
    {
        var course = await dbContext.Courses
            .AsNoTracking() // Avoid tracking for read-only queries
            .Include(c=>c.Instructor)
            .Include(c => c.CourseSections) // Include sections
            .ThenInclude(cs => cs.Lessons) // Include lessons within sections
            .ThenInclude(cl => cl.CourseLessonResources) // Include materials within lessons
            .Include(c => c.Categories) // Include categories
            .FirstOrDefaultAsync(c => c.CourseId == id);

        if (course == null)
        {
            throw new ResourceNotFound(nameof(Domain.Entities.Course), id.ToString());
        }

        // // Sort sections, lessons, and materials
        // course.CourseSections = course.CourseSections
        //     .OrderBy(cs => cs.Order)
        //     .Select(cs =>
        //     {
        //         cs.Lessons = cs.Lessons
        //             .OrderBy(cl => cl.Order)
        //             .Select(cl =>
        //             {
        //                 cl.CourseMateriels = cl.CourseMateriels
        //                     .OrderBy(cm => cm.ItemOrder)
        //                     .ToList();
        //                 return cl;
        //             }).ToList();
        //         return cs;
        //     }).ToList();
        //
        // course.CourseMateriels = course.CourseMateriels
        //     .OrderBy(cm => cm.ItemOrder)
        //     .ToList();

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


    public async Task<(int, IEnumerable<Domain.Entities.Course>)> GetAllAsync(string? searchName, int requestPageNumber,
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