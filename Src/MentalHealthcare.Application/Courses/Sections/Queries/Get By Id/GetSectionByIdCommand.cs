using MediatR;
using MentalHealthcare.Domain.Dtos.course;

namespace MentalHealthcare.Application.Courses.Sections.Queries.Get_By_Id;

public class GetSectionByIdCommand:IRequest<CourseSectionDto>
{
    [System.Text.Json.Serialization.JsonIgnore]

    public int CourseId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]

    public int SectionId { get; set; }
}