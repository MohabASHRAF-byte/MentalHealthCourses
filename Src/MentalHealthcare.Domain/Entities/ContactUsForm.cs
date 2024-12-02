using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Domain.Entities;

public class ContactUsForm
{
    public int ContactUsFormId { get; set; }
    [MaxLength(Global.ContactUsMaxNameLength)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(Global.ContactUsMaxEmailLength)]
    public string? Email { get; set; }
    [MaxLength(Global.ContactUsMaxPhoneLength)]
    public string? PhoneNumber { get; set; }
    [MaxLength(Global.ContactUsMaxMsgLength)]
    public string Message { get; set; }=string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedDate { get; set; }
}