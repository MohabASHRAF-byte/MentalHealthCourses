using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Domain.Dtos;

public class AdvertisementDto
{
    public int AdvertisementId { get; set; }
    public List<string> AdvertisementImageUrls { get; set; } = [];
    [MaxLength(Global.AdvertisementNameMaxLength)]
    public string AdvertisementName { get; set; }
    [MaxLength(Global.AdvertisementDescriptionMaxLength)]
    public string AdvertisementDescription { get; set; } 
    public bool IsActive { get; set; } 
}