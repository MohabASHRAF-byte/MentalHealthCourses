using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos;

namespace MentalHealthcare.Application.Courses.Favourite.Queries.GetFavouriteCourse;

public class GetFavouriteCoursesQuery : IRequest<PageResult<CourseViewDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; } = "";
}