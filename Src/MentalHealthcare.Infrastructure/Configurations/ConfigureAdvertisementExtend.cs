using MentalHealthcare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Configurations;

public static class ConfigureAdvertisementExtend
{
    public static void ConfigureAdvertisement(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Advertisement>()
            .HasMany(a => a.AdvertisementImageUrls)
            .WithOne(ai => ai.Advertisement)
            .HasForeignKey(ai => ai.AdvertisementId);
    }
}