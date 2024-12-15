using System.ComponentModel.DataAnnotations;
using MediatR;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Application.HelpCenterItem.Commands.Create;

public class CreateHelpCenterItemCommand:IRequest<int>
{
    public Global.HelpCenterItems HelpCenterItemType { get; set; }
    [MaxLength(Global.TermNameMaxLength)] 
    public string Name { get; set; } = string.Empty;

    

    [MaxLength(Global.TermDescriptionMaxLength)]
    public string Description { get; set; } = string.Empty;
}