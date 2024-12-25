using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Entities.OrderProcessing;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Configurations;

public static class ConfigureCartExtend
{
    public static void ConfigureCart(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CoursesCart>(entity =>
        {

            entity.HasMany(c => c.Items)
                .WithOne(i => i.Cart)
                .HasForeignKey(i => i.CoursesCartId)
                .OnDelete(DeleteBehavior.Cascade); // Ensure cascade delete
        });
    }
}