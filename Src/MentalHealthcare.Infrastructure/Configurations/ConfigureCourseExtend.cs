using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Entities.Courses;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Configurations;

public static class ConfigureCourseExtend
{
    public static void ConfigureCourse(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(c => c.CourseId);

            // Each course must have one instructor
            entity.HasOne(c => c.Instructor)
                .WithMany(i => i.Courses)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Cascade); // Delete courses when the instructor is deleted

            // Many-to-Many: Categories
            entity.HasMany(c => c.Categories)
                .WithMany(cat => cat.Courses);


        });
    }
}