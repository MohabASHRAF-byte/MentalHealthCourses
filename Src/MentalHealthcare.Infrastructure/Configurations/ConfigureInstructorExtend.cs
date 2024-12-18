using MentalHealthcare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Configurations;

public static class ConfigureInstructorExtend
{
    public static void ConfigureInstructor(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.HasKey(i => i.InstructorId);

            // Configure auto-generated identity for InstructorId
            entity.Property(i => i.InstructorId).UseIdentityColumn(1, 1);

            // An instructor can have many courses
            entity.HasMany(i => i.Courses)
                .WithOne(c => c.Instructor)  // One instructor for each course
                .HasForeignKey(c => c.InstructorId) // Define the foreign key on the Course
                .OnDelete(DeleteBehavior.Cascade);  // Deleting an instructor will cascade delete their courses
        });
    }

}