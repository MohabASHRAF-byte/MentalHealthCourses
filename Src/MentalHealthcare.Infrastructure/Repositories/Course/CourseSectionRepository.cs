using MentalHealthcare.Application.Courses.Sections.Commands.Update_order;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using MentalHealthcare.Infrastructure.Persistence;
using MentalHealthcare.Infrastructure.scripts;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Repositories.Course;

public class CourseSectionRepository(
    MentalHealthDbContext dbContext
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


    public async Task UpdateCourseSectionsAsync(int courseId, List<SectionOrderDto> orders)
    {
        // Fetch existing sections
        var sections = await dbContext.CourseSections
            .Where(cs => cs.CourseId == courseId)
            .ToListAsync();

        // Check if the course is already joined
        var isAnyOneJoined = await dbContext.CourseProgresses
            .AnyAsync(cps => cps.CourseId == courseId);
        if (isAnyOneJoined)
        {
            throw new ArgumentException("Course sections cannot be updated as users have joined the course.");
        }

        if (sections == null || sections.Count == 0)
        {
            throw new InvalidOperationException($"Course {courseId} has no sections.");
        }

        // Validate order input
        var sectionIds = sections.Select(s => s.CourseSectionId).ToHashSet();
        var requestSectionIds = orders.Select(o => o.SectionId).ToHashSet();
        if (!sectionIds.SetEquals(requestSectionIds))
        {
            throw new ArgumentException("The request does not contain valid orders for all sections.");
        }

        var orderValues = orders.Select(o => o.Order).OrderBy(o => o).ToList();
        if (!orderValues.SequenceEqual(Enumerable.Range(1, sections.Count)))
        {
            throw new ArgumentException("Order values must be sequential from 1 to the total number of sections.");
        }

        // Begin transaction
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            // Update section orders
            var changesMade = false;
            foreach (var section in sections)
            {
                var newSectionOrder = orders.First(o => o.SectionId == section.CourseSectionId).Order;
                if (section.Order != newSectionOrder)
                {
                    section.Order = newSectionOrder;
                    changesMade = true;
                }
            }

            if (changesMade)
            {
                dbContext.CourseSections.UpdateRange(sections);
                // Update lessons' order based on section updates

                await dbContext.UpdateCourseLessonsOrder(courseId);

                await dbContext.SaveChangesAsync();
            }


            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new InvalidOperationException("Error updating course sections.", ex);
        }
    }

    public async Task DeleteCourseSectionIfEmptyAsync(
        int courseId,
        int sectionId
    )
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            // Fetch the section to delete
            var section = await dbContext.CourseSections
                .Where(cs => cs.CourseId == courseId)
                .FirstOrDefaultAsync();
            if (section == null)
            {
                throw new ResourceNotFound(nameof(CourseSection), sectionId.ToString());
            }

            // Check if the section is empty
            var isSectionEmpty = !await dbContext.CourseLessons
                .AnyAsync(cl => cl.CourseSectionId == sectionId);
            if (!isSectionEmpty)
            {
                throw new TryAgain("Section should be empty to delete.");
            }

            // Fetch all sections for the course
            var sections = await dbContext.CourseSections
                .Where(c => c.CourseId == courseId)
                .OrderBy(c => c.Order)
                .ToListAsync();

            // Adjust the order of sections after the deleted one
            foreach (var nSection in sections)
            {
                if (nSection.Order > section.Order)
                {
                    nSection.Order--;
                }
            }

            // Remove the section
            dbContext.CourseSections.Remove(section);

            // Save changes and commit transaction
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception("An error occurred while deleting the course section.", ex);
        }
    }


    public async Task<CourseSection> GetCourseSectionByIdAsync(
        int courseId,
        int sectionId
    )
    {
        var courseSection = await dbContext.CourseSections
            .Include(cs => cs.Lessons)
            .ThenInclude(l => l.CourseLessonResources)
            .FirstOrDefaultAsync(
                c =>
                    c.CourseSectionId == sectionId
                    && c.CourseId == courseId
            );
        if (courseSection == null)
        {
            throw new ResourceNotFound(nameof(CourseSection), sectionId.ToString());
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

    public async Task<IEnumerable<CourseSectionViewDto>> GetCourseSectionsByCourseIdAsync(int courseId)
    {
        // Fetch and project course sections in a single query
        var courseSections = await dbContext.CourseSections
            .Where(cs => cs.CourseId == courseId)
            .OrderBy(cs => cs.Order)
            .Select(cs => new CourseSectionViewDto
            {
                CourseSectionId = cs.CourseSectionId,
                Name = cs.Name,
                Order = cs.Order,
                LessonsCount = cs.Lessons.Count
            })
            .ToListAsync();

        return courseSections;
    }


    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}