using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Application.ContactUs.Queries.GetAll;

public class GetAllContactFormsQuery : IRequest<PageResult<ContactUsForm>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int ViewMsgLengthLimiter { get; set; } = 50;
    public string? SenderName { get; set; }
    public string? SenderEmail { get; set; }
    public string? SenderPhone { get; set; }
    public bool? IsRead { set; get; }
}