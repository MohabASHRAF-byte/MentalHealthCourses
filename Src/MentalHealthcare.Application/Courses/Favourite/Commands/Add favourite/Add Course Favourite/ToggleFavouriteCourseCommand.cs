using MediatR;

namespace MentalHealthcare.Application.Courses.Favourite.Commands.Add_favourite.Add_Course_Favourite;

public class ToggleFavouriteCourseCommand:IRequest
{
    public int CourseId { get; set; }
}