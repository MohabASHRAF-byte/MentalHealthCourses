using MediatR;

namespace MentalHealthcare.Application.Courses.Favourite.Commands.Toggle_favourite;

public class ToggleFavouriteCourseCommand:IRequest
{
    public int CourseId { get; set; }
}