using System.Text.Json.Serialization;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos.course;

namespace MentalHealthcare.Application.Courses.Sections.Queries.Get_All;

public class GetAllCourseSectionsQuery:IRequest<IEnumerable<CourseSectionViewDto> >
{
    [JsonIgnore] 
    public int courseId { get; set; }

}