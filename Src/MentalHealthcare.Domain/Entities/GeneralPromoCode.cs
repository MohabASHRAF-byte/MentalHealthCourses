namespace MentalHealthcare.Domain.Entities;

public class GeneralPromoCode

{
    public int GeneralPromoCodeId { get; set; }

    public string Code { get; set; }

    public DateTime expiredate { get; set; }

    public float percentage { get; set; }

    public bool isActive { get; set; }
}