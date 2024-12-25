using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Application.Courses.Favourite.Queries.GetUsersWhoFavouriteCourse;

public class GetUsersWhoFavouriteCourseQuery : IRequest<PageResult<SystemUser>>
{
    public int CourseId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
}