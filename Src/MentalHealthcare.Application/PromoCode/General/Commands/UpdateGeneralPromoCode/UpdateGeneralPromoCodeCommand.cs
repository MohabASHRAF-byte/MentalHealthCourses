using System.Text.Json.Serialization;
using MediatR;

namespace MentalHealthcare.Application.PromoCode.General.Commands.UpdateGeneralPromoCode;

public class UpdateGeneralPromoCodeCommand:IRequest
{
    [JsonIgnore]
    public int GeneralPromoCodeId { get; set; }
    
    public string? ExpireDate { get; set; }

    public double? Percentage { get; set; }
}