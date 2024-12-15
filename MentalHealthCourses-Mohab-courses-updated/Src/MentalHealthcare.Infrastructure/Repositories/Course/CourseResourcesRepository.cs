using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Infrastructure.Repositories.Course;

public class CourseResourcesRepository(
    MentalHealthDbContext dbContext,
    ILogger<CourseResourcesRepository> logger
) : ICourseResourcesRepository
{
    public async Task<CourseLessonResource> AddCourseLessonResourceAsync(CourseLessonResource resource)
    {
        var maxOrder = await dbContext.CourseLessonResources
            .Where(c => c.CourseLessonId == resource.CourseLessonId)
            .MaxAsync(c => (int?)c.ItemOrder) ?? 0;

        resource.ItemOrder = maxOrder + 1;
        dbContext.CourseLessonResources.Add(resource);
        await dbContext.SaveChangesAsync();
        return resource;
    }

    public async Task<CourseLessonResource> GetCourseLessonResourceByIdAsync(int id)
    {
        var courseLessonResource = await dbContext.CourseLessonResources
            .FirstOrDefaultAsync(c => c.CourseLessonResourceId == id);
        if (courseLessonResource == null)
        {
            throw new ResourceNotFound(nameof(courseLessonResource), id.ToString());
        }

        return courseLessonResource;
    }

    public async Task<List<CourseLessonResource>> GetCourseLessonResourcesByCourseIdAsync(int lessonId)
    {
        var courseLessonResources =
            await dbContext.CourseLessonResources
                .Where(cl => cl.CourseLessonId == lessonId)
                .OrderBy(cr => cr.ItemOrder)
                .ToListAsync();
        return courseLessonResources;
    }

    public async Task DeleteCourseLessonResourceAsync(CourseLessonResource resource)
    {
        dbContext.CourseLessonResources.Remove(resource);
        await dbContext.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateCourseLessonResourcesAsync(List<CourseLessonResource> resources)
    {
        dbContext.UpdateRange(resources);
        await dbContext.SaveChangesAsync();
    }
}