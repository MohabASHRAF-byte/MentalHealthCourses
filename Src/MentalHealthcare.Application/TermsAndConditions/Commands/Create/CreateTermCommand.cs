using System.ComponentModel.DataAnnotations;
using MediatR;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Application.TermsAndConditions.Commands.Create;

public class CreateTermCommand:IRequest<int>
{
    [MaxLength(Global.TermNameMaxLength)] 
    public string Name { get; set; } = string.Empty;

    [MaxLength(Global.TermDescriptionMaxLength)]
    public string Description { get; set; } = string.Empty;
}