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

    public async Task<Course> GetByIdAsync(int id)
    {
        var course = await dbContext.Courses
            .AsNoTracking() // Avoid tracking for read-only queries
            .Include(c=>c.Instructor)
            .Include(c => c.CourseSections) // Include sections
            .ThenInclude(cs => cs.Lessons) // Include lessons within sections
            .ThenInclude(cl => cl.CourseMateriels) // Include materials within lessons
            .Include(c => c.CourseMateriels) // Include materials directly under the course
            .Include(c => c.Categories) // Include categories
            .Include(c => c.UsersFavCourse) // Include users who favorited the course
            .FirstOrDefaultAsync(c => c.CourseId == id);

        if (course == null)
        {
            throw new ResourceNotFound(nameof(Course), id.ToString());
        }

        // Sort sections, lessons, and materials
        course.CourseSections = course.CourseSections
            .OrderBy(cs => cs.Order)
            .Select(cs =>
            {
                cs.Lessons = cs.Lessons
                    .OrderBy(cl => cl.Order)
                    .Select(cl =>
                    {
                        cl.CourseMateriels = cl.CourseMateriels
                            .OrderBy(cm => cm.ItemOrder)
                            .ToList();
                        return cl;
                    }).ToList();
                return cs;
            }).ToList();

        course.CourseMateriels = course.CourseMateriels
            .OrderBy(cm => cm.ItemOrder)
            .ToList();

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

    public async Task<CourseSection> GetCourseSectionByIdAsync(int id)
    {
        var courseSection = await dbContext.CourseSections
            .FirstOrDefaultAsync(c => c.CourseSectionId == id);
        if (courseSection == null)
        {
            throw new ResourceNotFound(nameof(CourseSection), id.ToString());
        }

        return courseSection;
    }

    public async Task<int> AddCourseLesson(CourseLesson courseLesson)
    {
        var maxOrder = await dbContext.CourseLessons
            .Where(c => c.CourseId == courseLesson.CourseId
                        && c.CourseSectionId == courseLesson.CourseSectionId
            )
            .MaxAsync(c => (int?)c.Order) ?? 0;

        courseLesson.Order = maxOrder + 1;
        dbContext.CourseLessons.Add(courseLesson);
        await dbContext.SaveChangesAsync();
        return courseLesson.CourseLessonId;
    }

    public async Task<List<CourseLesson>> GetCourseLessons(int courseId, int sectionId)
    {
        var lessons = dbContext.CourseLessons
            .Where(c => c.CourseId == courseId
                        && c.CourseSectionId == sectionId
            );
        return await lessons.ToListAsync();
    }

    public async Task DeleteCourseLessonAsync(CourseLesson targetLesson)
    {
        dbContext.Remove(targetLesson);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateCourseLessonsAsync(List<CourseLesson> updatedLessons)
    {
        dbContext.CourseLessons.UpdateRange(updatedLessons);
        await dbContext.SaveChangesAsync();
    }

    public async Task<CourseLesson> GetCourseLessonByIdAsync(int id)
    {
        var courseLesson = await dbContext.CourseLessons
            .FirstOrDefaultAsync(c => c.CourseLessonId == id);
        if (courseLesson == null)
        {
            throw new ResourceNotFound(nameof(courseLesson), id.ToString());
        }
        return courseLesson;
        
    }

    public async Task<CourseMateriel> GetCourseMaterielByIdAsync(int id)
    {
        var courseMateriel = await dbContext.CourseMateriels
            .FirstOrDefaultAsync(c => c.CourseMaterielId == id);
        if (courseMateriel == null)
        {
            throw new ResourceNotFound(nameof(courseMateriel), id.ToString());
        }
        return courseMateriel;
    }
}