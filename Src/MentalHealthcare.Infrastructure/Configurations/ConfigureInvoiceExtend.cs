using MentalHealthcare.Domain.Entities.OrderProcessing;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Configurations;

public static class ConfigureInvoiceExtend
{
    public static void ConfigureInvoice(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Invoice>(entity =>
        {
            // Primary Key
            entity.HasKey(i => i.InvoiceId);

            // Configure auto-generated identity for InvoiceId
            entity.Property(i => i.InvoiceId)
                .UseIdentityColumn(1, 1);

            // Relationships
            entity.HasOne(i => i.User)
                .WithMany()
                .HasForeignKey(i => i.UserID)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cycles/multiple cascade paths

            entity.HasOne(i => i.SystemUser)
                .WithMany()
                .HasForeignKey(i => i.SystemUserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cycles/multiple cascade paths


            entity.HasOne(i => i.Admin)
                .WithMany()
                .HasForeignKey(i => i.AdminId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cycles/multiple cascade paths

            // Property Configurations
            entity.Property(i => i.Notes)
                .HasMaxLength(500); // Adjust max length as needed

            entity.Property(i => i.OrderDate)
                .HasDefaultValueSql("GETDATE()"); // Set default value to current UTC date
        });
    }
}