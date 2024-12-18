using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Configurations;
public static class ConfigurePodcastsExtend
{
    public static void ConfigurePodcast(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Podcast>(entity =>
        {
            entity.HasKey(p => p.PodcastId);

            entity.Property(p => p.PodcastId)
                .UseIdentityColumn(100, 20);

            entity.Property(p => p.Title)
                .IsRequired()
                .HasAnnotation("DataType", DataType.Text);

            entity.Property(p => p.UploadedById)
                .IsRequired();

            entity.Property(p => p.CreatedDate)
                .IsRequired()
                .HasAnnotation("DataType", DataType.DateTime)
                .HasDefaultValueSql("GETDATE()");

            entity.Property(p => p.PodcastLength)
                .IsRequired();

            // Configure relationships
            entity.HasOne(p => p.UploadedBy)
                .WithMany(a => a.Podcasts)
                .HasForeignKey(p => p.UploadedById)
                .OnDelete(DeleteBehavior.Restrict); // No cascading delete for UploadedBy

            entity.HasOne(p => p.PodCaster)
                .WithMany(pc => pc.Podcasts)
                .HasForeignKey(p => p.PodCasterId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete when PodCaster is deleted
        });
    }
}
