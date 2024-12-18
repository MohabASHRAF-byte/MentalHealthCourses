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
            });
        }
    }
}