using MentalHealthcare.Domain.Entities.Courses;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Configurations;

public static class CourseProgressConfigurationExtend{
    
    public static void ConfigureCourseProgress(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CourseProgress>(entity =>
        {
            #region Primary Key
            entity.HasKey(cp => cp.CourseProgressId);
            #endregion

            #region Relationships
            entity.HasOne(cp => cp.SystemUser)
                .WithMany()
                .HasForeignKey(cp => cp.SystemUserId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(cp => cp.Course)
                .WithMany()
                .HasForeignKey(cp => cp.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion
            
        });
    }
}