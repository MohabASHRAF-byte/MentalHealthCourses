using System.Text.Json.Serialization;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos.User;

namespace MentalHealthcare.Application.Courses.Course.Queries.EnrolledUsers;

public class GetEnrolledUsersQuery:IRequest<PageResult<GetUserProfile>>
{
    [JsonIgnore]
    public int CourseId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
}