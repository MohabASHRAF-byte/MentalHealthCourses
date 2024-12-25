using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Entities.Courses;

namespace MentalHealthcare.Domain.Repositories.Course;

public interface ICourseFavouriteRepository
{
    public Task ToggleFavouriteCourseAsync(int courseId, int userId);

    public Task<(int count, List<CourseViewDto> courses)> GetUserFavourites
    (
        int userId,
        int pageNumber,
        int pageSize,
        string searchTerm
    );

    // public Task<(int count, List<SystemUser> courses)> GetUsersWhoFavouriteCourse(
    //     int courseId, int pageNumber,
    //     int pageSize, string? searchTerm
    // );
}