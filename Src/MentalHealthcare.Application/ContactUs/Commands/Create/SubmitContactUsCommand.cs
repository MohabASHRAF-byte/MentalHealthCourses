using System.ComponentModel.DataAnnotations;
using MediatR;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Application.ContactUs.Commands.Create;

public class SubmitContactUsCommand:IRequest<int>
{
    public string Name { get; set; } = string.Empty;
    [MaxLength(Global.ContactUsMaxEmailLength)]
    public string? Email { get; set; }
    [MaxLength(Global.ContactUsMaxPhoneLength)]
    public string? PhoneNumber { get; set; }
    [MaxLength(Global.ContactUsMaxMsgLength)]
    public string Message { get; set; }=string.Empty;
    public bool IsRead { get; set; }
}