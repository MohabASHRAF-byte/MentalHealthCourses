using MentalHealthcare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Configurations;
public static class ConfigurePodCasterExtend
{
    public static void ConfigurePodCaster(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PodCaster>(entity =>
        {
            entity.HasKey(p => p.PodCasterId);

            entity.Property(p => p.PodCasterId)
                .UseIdentityColumn(100, 1);

            entity.HasMany(p => p.Podcasts)
                .WithOne(pc => pc.PodCaster)
                .HasForeignKey(pc => pc.PodCasterId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete when PodCaster is deleted
        });
    }
}
