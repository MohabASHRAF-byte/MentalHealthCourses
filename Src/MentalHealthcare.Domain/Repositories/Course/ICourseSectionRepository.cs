using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories.Course;

public interface ICourseSectionRepository
{
    public Task IsSectionExistAndUpdatableAsync(int courseId, int sectionId);
    public Task<int> AddCourseSection(CourseSection courseSection);

    public Task<List<CourseSection>> GetCourseSections(int courseId);

    public Task UpdateCourseSectionsAsync(
        int courseId,
        List<SectionOrderDto> orders
    );
    public  Task DeleteCourseSectionIfEmptyAsync(
        int courseId,
        int sectionId
    );

    public  Task<CourseSection> GetCourseSectionByIdAsync(
        int courseId,
        int sectionId
    );
    public Task<CourseSection> GetCourseSectionByIdUnTrackedAsync(int id);
    public Task<IEnumerable<CourseSectionViewDto>> GetCourseSectionsByCourseIdAsync(
        int courseId
        );
    public Task SaveChangesAsync();
}