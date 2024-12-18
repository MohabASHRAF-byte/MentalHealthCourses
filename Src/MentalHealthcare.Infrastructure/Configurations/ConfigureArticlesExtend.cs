using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Configurations
{
    public static class ConfigureArticlesExtend
    {
        public static void ConfigureArticle(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>(entity =>
            {
                // Primary key for Article
                entity.HasKey(a => a.ArticleId);

                // Configure ArticleId to use identity columns
                entity.Property(a => a.ArticleId).UseIdentityColumn(10, 10);

                // Configure Title property
                entity.Property(a => a.Title)
                    .IsRequired()
                    .HasAnnotation("DataType", DataType.Text);

                // Configure foreign keys
                // Make UploadedById nullable to allow NULL values
                entity.Property(a => a.UploadedById)
                    .IsRequired(false); // Allows NULL values

                // Ensure AuthorId is required
                entity.Property(a => a.AuthorId)
                    .IsRequired();

                // Configure CreatedDate property
                entity.Property(a => a.CreatedDate)
                    .IsRequired()
                    .HasAnnotation("DataType", DataType.DateTime)
                    .HasDefaultValueSql("GETDATE()"); 

                // Configure relationships
                // Configure UploadedBy (Admin) relationship
                // Configure Author relationship (Cascade delete for Article)
                entity.HasOne(a => a.Author) // Relationship to Author
                    .WithMany(au => au.Articles) // One-to-many relationship
                    .HasForeignKey(a => a.AuthorId) // Foreign key property
                    .OnDelete(DeleteBehavior.Cascade); // Cascade delete when Author is deleted

                // You can configure other relationships as needed
            });
        }
    }
}
