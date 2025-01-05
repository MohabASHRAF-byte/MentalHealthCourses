using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos.course;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MentalHealthcare.Application.Courses.Course_Interactions.Queries.GetMyCourses;

public class GetMyCoursesQuery : IRequest<PageResult<CourseActivityDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; } = "";
}