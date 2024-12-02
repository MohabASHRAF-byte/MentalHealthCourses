using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Domain.Entities;

public class Advertisement
{
    public int AdvertisementId { get; set; }
    public List<AdvertisementImageUrl> AdvertisementImageUrls { get; set; } = [];

    [MaxLength(Global.AdvertisementNameMaxLength)]
    public string AdvertisementName { get; set; }

    [MaxLength(Global.AdvertisementDescriptionMaxLength)]
    public string AdvertisementDescription { get; set; }

    public bool IsActive { get; set; }
    public int LastUploadImgCnt { get; set; } = 0;
}