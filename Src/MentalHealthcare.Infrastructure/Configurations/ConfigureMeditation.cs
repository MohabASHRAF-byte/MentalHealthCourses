using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Configurations;

public static class ConfigureMeditationExtend
{
    public static void ConfigureMeditation(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Meditation>(entity =>
        {
            entity.HasKey(m => m.MeditationId);
            entity.Property(m => m.MeditationId).UseIdentityColumn(110, 10);
            entity.Property(m => m.Title).IsRequired().HasAnnotation("DataType", DataType.Text);
            entity.Property(m => m.Content).IsRequired();
            entity.Property(m => m.CreatedDate).IsRequired().HasAnnotation("DataType", DataType.DateTime);
            entity.Property(m => m.CreatedDate)
                .HasDefaultValueSql("GETDATE()"); // Set the default value in the table instead of generating it.

            // Use UploadedById for foreign key configuration
            entity.HasOne(m => m.UploadedBy)
                .WithMany(a => a.Meditations) // Make sure the Admin class has a Meditations collection
                .HasForeignKey(m => m.UploadedById) // Correctly reference the foreign key property
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

}