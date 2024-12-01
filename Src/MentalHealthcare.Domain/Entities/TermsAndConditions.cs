using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Domain.Entities;

public class TermsAndConditions
{
    public int TermsAndConditionsId { get; set; }
    [MaxLength(Global.TermNameMaxLength)]
    public string Name { get; set; } =string.Empty;
    [MaxLength(Global.TermDescriptionMaxLength)]
    public string Description { get; set; } =string.Empty;
}