using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Domain.Entities;

public class HelpCenterItem
{
    public int HelpCenterItemId { get; set; }
    public Global.HelpCenterItems Type { set; get; }
    [MaxLength(Global.TermNameMaxLength)]
    public string Name { get; set; } =string.Empty;
    [MaxLength(Global.TermDescriptionMaxLength)]
    public string Description { get; set; } =string.Empty;
}