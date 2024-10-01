using MentalHealthcare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Persistence;

public class MentalHealthDbContext(
    DbContextOptions<MentalHealthDbContext> options
) : DbContext(options)
{
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseMateriel> CourseMateriels { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>()
            .HasMany(c => c.CourseMateriels)
            .WithOne(cm => cm.Course)
            .HasForeignKey(cm => cm.CourseId)
            .OnDelete(DeleteBehavior.Cascade);  
    
        base.OnModelCreating(modelBuilder);
    }
}