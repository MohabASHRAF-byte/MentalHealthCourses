using MentalHealthcare.Domain.Entities.Courses;

namespace MentalHealthcare.Domain.Repositories.Course;

public interface ICourseFavouriteRepository
{
    public Task ToggleFavouriteCourseAsync(int courseId, string userId);
    
}