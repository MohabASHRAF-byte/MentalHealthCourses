using MentalHealthcare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Configurations;

public static class ConfigureCoursePromoCodeExtend
{
    public static void CoursePromoCodeConfiguration(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CoursePromoCode>(entity =>
        {
            // Add a unique constraint on Code and CourseId
            entity.HasIndex(e => new { e.Code, e.CourseId })
                .IsUnique();

        });
    }
}