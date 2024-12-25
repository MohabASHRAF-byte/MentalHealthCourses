using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities.Courses;

namespace MentalHealthcare.Domain.Repositories.Course;

public interface ICourseFavouriteRepository
{
    public Task ToggleFavouriteCourseAsync(int courseId, string userId);

    public Task<(int count, List<CourseViewDto> courses)> GetUserFavourites
    (
        string userId,
        int pageNumber,
        int pageSize,
        string searchTerm
    );
}