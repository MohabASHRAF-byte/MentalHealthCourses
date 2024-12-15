using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories.Course;

public interface ICourseSectionRepository
{
    public Task IsExistAsync(int courseId, int sectionId);
    public Task<int> AddCourseSection(CourseSection courseSection);

    public Task<List<CourseSection>> GetCourseSections(int courseId);
    public Task UpdateCourseSectionsAsync(List<CourseSection> courseSection);

    public Task DeleteCourseSectionAsync(CourseSection courseSection);
    public Task<CourseSection> GetCourseSectionByIdAsync(int id);
    public Task<CourseSection> GetCourseSectionByIdUnTrackedAsync(int id);
    public Task<(int, IEnumerable<CourseSectionViewDto>)> GetCourseSectionsByCourseIdAsync(
        int courseId,
        string? search, int requestPageNumber, int requestPageSize
        );
    public Task SaveChangesAsync();
}