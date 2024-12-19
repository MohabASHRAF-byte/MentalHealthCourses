using MediatR;

namespace MentalHealthcare.Application.PromoCode.General.Commands.DeleteGeneralPromoCode;

public class DeleteGeneralPromoCodeCommand:IRequest
{
    [System.Text.Json.Serialization.JsonIgnore]
    public int GeneralPromoCodeId { get; set; }
}