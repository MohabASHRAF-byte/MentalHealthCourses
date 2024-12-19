namespace MentalHealthcare.Domain.Dtos.PromoCode;

public class GeneralPromoCodeDto
{
    public int GeneralPromoCodeId { get; set; }
    public string Code { get; set; }
    public DateTime expiredate { get; set; }
    public int expiresInDays { get; set; }
    public float percentage { get; set; }

}