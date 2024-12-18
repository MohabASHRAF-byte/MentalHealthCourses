using MentalHealthcare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Configurations;

public static class ConfigureCourseLessonExtend
{
    public static void ConfigureCourseLesson(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CourseLesson>(entity =>
        {
            entity.HasKey(e => e.CourseLessonId);

            entity.Property(e => e.LessonName)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.MaterielBunneyId)
                .IsRequired();

            entity.Property(e => e.Url)
                .IsRequired();

            entity.Property(e => e.LessonBunnyName)
                .IsRequired();

            // Configure relationship with CourseSection
            entity.HasOne(e => e.CourseSection)
                .WithMany(cs => cs.Lessons)
                .HasForeignKey(e => e.CourseSectionId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

            // Configure relationship with Admin
            entity.HasOne(e => e.Admin)
                .WithMany()
                .HasForeignKey(e => e.AdminId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete
        });
    }
}