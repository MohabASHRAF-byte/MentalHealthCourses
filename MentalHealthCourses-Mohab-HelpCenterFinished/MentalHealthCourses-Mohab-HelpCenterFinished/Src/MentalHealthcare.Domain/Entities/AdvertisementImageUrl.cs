
using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Domain.Entities;

public class AdvertisementImageUrl
{
    public int AdvertisementImageUrlId { get; set; }

    [MaxLength(Global.UrlMaxLength)] // Limit URL length
    public string ImageUrl { get; set; }
    public int AdvertisementId { get; set; } // Foreign key to Advertisement
    public Advertisement Advertisement { get; set; } // Navigation property
}
