using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Infrastructure.Repositories.Course;

public class CourseSectionRepository(
    MentalHealthDbContext dbContext,
    ILogger<CourseRepository> logger
) : ICourseSectionRepository
{
    public async Task IsExistAsync(int courseId, int sectionId)
    {
        var exists = await dbContext.CourseSections
            .AsNoTracking()
            .AnyAsync(c => c.CourseSectionId == sectionId);

        if (!exists)
        {
            throw new ResourceNotFound(nameof(CourseSection), sectionId.ToString());
        }
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
            .Include(cs=>cs.Lessons)
            .ThenInclude(l=>l.CourseLessonResources)
            .FirstOrDefaultAsync(c => c.CourseSectionId == id);
        if (courseSection == null)
        {
            throw new ResourceNotFound(nameof(CourseSection), id.ToString());
        }
        foreach (var lesson in courseSection.Lessons)
        {
            if (lesson.CourseLessonResources != null)
                lesson.CourseLessonResources = lesson.CourseLessonResources
                    .OrderBy(r => r.ItemOrder) // Replace 'SomeProperty' with the field to sort by
                    .ToList();
        }
        return courseSection;
    }
    public async Task<CourseSection> GetCourseSectionByIdUnTrackedAsync(int id)
    {
        var courseSection = await dbContext.CourseSections
            .AsNoTracking()
            .Include(cs => cs.Lessons)
            .ThenInclude(l => l.CourseLessonResources)
            .FirstOrDefaultAsync(c => c.CourseSectionId == id);
        if (courseSection == null)
        {
            throw new ResourceNotFound(nameof(CourseSection), id.ToString());
        }
        foreach (var lesson in courseSection.Lessons)
        {
            if (lesson.CourseLessonResources != null)
                lesson.CourseLessonResources = lesson.CourseLessonResources
                    .OrderBy(r => r.ItemOrder) // Replace 'SomeProperty' with the field to sort by
                    .ToList();
        }
        return courseSection;
    }

    public async Task<(int, IEnumerable<CourseSectionViewDto>)> GetCourseSectionsByCourseIdAsync(
        int courseId, 
        string? search, 
        int requestPageNumber, 
        int requestPageSize)
    {
        // Base query filtered by CourseId
        var baseQuery = dbContext.CourseSections
            .Where(cs => cs.CourseId == courseId)
            .AsQueryable();

        // Apply search filter if a search term is provided
        if (!string.IsNullOrWhiteSpace(search))
        {
            baseQuery = baseQuery.Where(cs => cs.Name.ToLower().Contains(search.ToLower()));
        }

        var totalCount = await baseQuery.CountAsync();

        var courseSections = await baseQuery
            .OrderBy(cs => cs.Order) 
            .Skip(requestPageSize * (requestPageNumber - 1)) 
            .Take(requestPageSize) 
            .Select(cs => new CourseSectionViewDto
            {
                CourseSectionId = cs.CourseSectionId,
                Name = cs.Name,
                Order = cs.Order,
                LessonsCount = cs.Lessons.Count 
            }).OrderBy(cs=>cs.Order)
            .ToListAsync();

        return (totalCount, courseSections);
    }


    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}
