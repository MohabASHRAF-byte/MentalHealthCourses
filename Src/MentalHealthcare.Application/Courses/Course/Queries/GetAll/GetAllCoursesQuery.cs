using System.ComponentModel.DataAnnotations;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos;

namespace MentalHealthcare.Application.Courses.Queries.GetAll;

public class GetAllCoursesQuery:IRequest<PageResult<CourseViewDto>>
{
    [MaxLength(100)]
    public string? SearchText { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}