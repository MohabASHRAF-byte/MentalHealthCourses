using MentalHealthcare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Configurations
{
    public static class ConfigureAuthorExtend
    {
        public static void ConfigureAuthor(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity =>
            {
                // Primary key configuration for Author
                entity.HasKey(a => a.AuthorId);
                entity.Property(a => a.AuthorId).UseIdentityColumn(10, 2);

                // Configure the relationship between Author and Articles
                entity.HasMany(a => a.Articles)
                .WithOne(ar => ar.Author) // One-to-many relationship
                    .HasForeignKey(ar => ar.AuthorId) // Use AuthorId as the foreign key
                    .OnDelete(DeleteBehavior.Cascade); // Cascade delete from Author to Articles



                // Configure Admin relationship (AddedBy) - Optional AdminId
                entity.HasOne(a => a.AddedBy)
            .WithMany(admin => admin.Authors)
            .HasForeignKey(a => a.AdminId)
            .OnDelete(DeleteBehavior.Restrict);








                //entity.HasOne(a => a.AddedBy) // One-to-one or many-to-one relationship
                //      .WithMany() // Assuming no navigation property in Admin for Authors
                //      .HasForeignKey(a => a.AdminId) // Use AdminId as the foreign key
                //      .OnDelete(DeleteBehavior.Restrict) // Prevent deletion of Admin if Author exists
                //      .IsRequired(false); // Make AdminId nullable


            });
        }
    }
}