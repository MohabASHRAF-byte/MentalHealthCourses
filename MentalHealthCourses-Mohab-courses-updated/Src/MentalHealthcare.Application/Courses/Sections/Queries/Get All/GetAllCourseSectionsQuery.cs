using System.Text.Json.Serialization;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos.course;

namespace MentalHealthcare.Application.Courses.Sections.Queries.Get_All;

public class GetAllCourseSectionsQuery:IRequest<PageResult<CourseSectionViewDto>>
{
    [JsonIgnore] 
    public int courseId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SearchString { get; set; } = string.Empty;
}