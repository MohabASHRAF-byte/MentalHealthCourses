using MediatR;

namespace MentalHealthcare.Application.PromoCode.General.Commands.AddGeneralPromoCode;

public class AddGeneralPromoCodeCommand:IRequest<int>
{
    public string Code { get; set; }

    public string ExpireDate { get; set; } 

    public float Percentage { get; set; }

}