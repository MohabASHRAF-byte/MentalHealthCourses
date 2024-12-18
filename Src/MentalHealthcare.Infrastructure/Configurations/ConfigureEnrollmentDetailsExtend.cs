using MentalHealthcare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Configurations
{
    public static class ConfigureEnrollmentDetailsExtend
    {
        public static void ConfigureEnrollmentDetails(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EnrollmentDetails>(entity =>
            {
                entity.HasKey(e => e.EnrollmentId);

                entity.Property(e => e.EnrollmentId)
                    .UseIdentityColumn(1, 1);

                // Set max length for Progress property
                entity.Property(e => e.Progress)
                    .IsRequired()
                    .HasMaxLength(200);

                // Configure relationship with SystemUser
                entity.HasOne(e => e.SystemUser)
                    .WithMany(u => u.EnrollmentDetails)
                    .HasForeignKey(e => e.SystemUserId)
                    .OnDelete(DeleteBehavior.Restrict); // Avoid cascading deletes

                // Configure relationship with Course
                entity.HasOne(e => e.Course)
                    .WithMany()
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Restrict); // Avoid cascading deletes
            });
        }
    }
}