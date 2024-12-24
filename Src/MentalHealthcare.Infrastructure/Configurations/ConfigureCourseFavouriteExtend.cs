using MentalHealthcare.Domain.Entities.Courses;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Configurations;

public static class ConfigureCourseFavouriteExtend
{
    public static void ConfigureCourseFavourite(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FavouriteCourse>(entity =>
        {
            entity.HasKey(fc => fc.FavouriteCourseId);

            entity.HasOne(fc => fc.Course)
                .WithMany()
                .HasForeignKey(fc => fc.CourseId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cycles or multiple cascade paths

            entity.HasOne(fc => fc.User)
                .WithMany()
                .HasForeignKey(fc => fc.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Delete favourites if the user is deleted

            entity.Property(fc => fc.FavouriteCourseId).IsRequired();
            entity.Property(fc => fc.CourseId).IsRequired();
            entity.Property(fc => fc.UserId).IsRequired();
        });
    }
}