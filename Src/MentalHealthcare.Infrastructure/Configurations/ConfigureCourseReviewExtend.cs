using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Infrastructure.Configurations;

public static class ConfigureCourseReviewExtend
{
    public static void ConfigureCourseReview(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserReview>(entity =>
        {
            entity.HasOne(ur => ur.User)
                .WithMany()
                .HasForeignKey(ur => ur.SystemUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(ur => ur.course)
                .WithMany()
                .HasForeignKey(ur => ur.courseId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}