using MentalHealthcare.Application.Courses.Sections.Commands.Update_order;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using MentalHealthcare.Infrastructure.Persistence;
using MentalHealthcare.Infrastructure.scripts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Repositories.Course;

public class CourseSectionRepository(
    MentalHealthDbContext dbContext,
    ILocalizationService localizationService
) : ICourseSectionRepository
{
    public async Task IsSectionExistAndUpdatableAsync(int courseId, int sectionId)
    {
        // Check if the section exists
        var exists = await dbContext.CourseSections
            .AsNoTracking()
            .AnyAsync(c => c.CourseSectionId == sectionId);

        if (!exists)
        {
            throw new ResourceNotFound(
                "Course Section", // English type name
                "قسم دورة تدريبية", // Alternative Arabic translation
                sectionId.ToString()
            );
        }

        // Check if the course has any students in progress
        var hasStudents = await dbContext.CourseProgresses
            .AnyAsync(progress => progress.CourseId == courseId);

        if (hasStudents)
        {
            // Get the max order of all sections in the course
            var maxOrder = await dbContext.CourseSections
                .Where(c => c.CourseId == courseId)
                .MaxAsync(c => c.Order);

            // Get the order of the current section
            var sectionOrder = await dbContext.CourseSections
                .Where(c => c.CourseSectionId == sectionId)
                .Select(c => c.Order)
                .FirstOrDefaultAsync();

            // Allow updating only if this is the last section
            if (sectionOrder != maxOrder)
            {
                throw new BadHttpRequestException(
                    "You can't update this section because the course already has students. " +
                    "Please try inserting it as the last section."
                );
            }
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
            
            throw new BadHttpRequestException(
                localizationService.GetMessage("CourseSectionsUpdateBlocked")
            );
        }

        if (sections == null || sections.Count == 0)
        {
            throw new BadHttpRequestException(
                string.Format(
                    localizationService.GetMessage("CourseHasNoSections", "Course {0} has no sections."),
                    courseId
                )
            );
        }

        // Validate order input
        var sectionIds = sections.Select(s => s.CourseSectionId).ToHashSet();
        var requestSectionIds = orders.Select(o => o.SectionId).ToHashSet();
        if (!sectionIds.SetEquals(requestSectionIds))
        {
            throw new BadHttpRequestException(
                localizationService.GetMessage("InvalidOrdersForSections")
            );
        }

        var orderValues = orders.Select(o => o.Order).OrderBy(o => o).ToList();
        if (!orderValues.SequenceEqual(Enumerable.Range(1, sections.Count)))
        {
            throw new BadHttpRequestException(
                localizationService.GetMessage("OrderValuesMustBeSequential")
            );
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
            throw new BadHttpRequestException(
                localizationService.GetMessage("ErrorUpdatingCourseSections", "Error updating course sections."),
                ex
            );
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
                throw new ResourceNotFound(
                    "Course Section", // English type name
                    "قسم دورة تدريبية", // Alternative Arabic translation
                    sectionId.ToString()
                );
            }

            // Check if the section is empty
            var isSectionEmpty = !await dbContext.CourseLessons
                .AnyAsync(cl => cl.CourseSectionId == sectionId);
            if (!isSectionEmpty)
            {
                throw new BadHttpRequestException(
                    localizationService.GetMessage("SectionShouldBeEmptyToDelete", "Section should be empty to delete.")
                );
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
            throw new BadHttpRequestException(
                localizationService.GetMessage("CourseSectionDeletionError"),
                ex
            );
        }
    }


    public async Task<CourseSection> GetCourseSectionByIdAsync(
        int courseId,
        int sectionId
    )
    {
        var courseSection = await dbContext.CourseSections
            .Include(cs => cs.Lessons.OrderBy(l => l.Order))
            .ThenInclude(l => l.CourseLessonResources)
            .FirstOrDefaultAsync(
                c =>
                    c.CourseSectionId == sectionId
                    && c.CourseId == courseId
            );
        if (courseSection == null)
        {
            throw new ResourceNotFound(
                "Course Section", // English type name
                "قسم دورة تدريبية", // Alternative Arabic translation
                sectionId.ToString()
            );
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
            .Include(cs => cs.Lessons.OrderBy(l => l.Order))
            .ThenInclude(l => l.CourseLessonResources)
            .FirstOrDefaultAsync(c => c.CourseSectionId == id);
        if (courseSection == null)
        {
            throw new ResourceNotFound(
                "Course Section", // English type name
                "قسم دورة تدريبية", // Alternative Arabic translation
                id.ToString()
            );
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