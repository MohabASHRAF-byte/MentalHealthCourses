using MentalHealthcare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Infrastructure.Configurations
{
    public static class ConfigureCertificateExtend
    {
        public static void ConfigureCertificate(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Certificate>(entity =>
            {
                // Primary key for Certificate
                entity.HasKey(c => c.CertificateId);

                // Configure CertificateId to use identity columns
                entity.Property(c => c.CertificateId)
                    .UseIdentityColumn(10, 10); // Set identity column properties

                // Configure UserEmail property
                entity.Property(c => c.UserEmail)
                    .IsRequired()  // Required field
                    .HasMaxLength(255);  // Max length for email

                // Configure UserName property
                entity.Property(c => c.UserName)
                    .IsRequired()  // Required field
                    .HasMaxLength(100);  // Max length for user name

                // Configure CourseName property
                entity.Property(c => c.CourseName)
                    .IsRequired()  // Required field
                    .HasMaxLength(255);  // Max length for course name

                // Configure CertificatePath property
                entity.Property(c => c.CertificatePath)
                    .IsRequired()  // Required field
                    .HasMaxLength(500);  // Max length for certificate path

                // Configure DateGenerated property
                entity.Property(c => c.DateGenerated)
                    .IsRequired()  // Required field
                    .HasDefaultValueSql("GETDATE()");  // Default value set to current date and time

                // Any other configurations you might need
                // For example, you could also add index or constraints as needed
                // Foreign Key Relationship (One Certificate -> Many Admins)
                entity.HasOne(c => c.Admin)  // A certificate is associated with one Admin
                    .WithMany(a => a.Certificates)  // An Admin can send many certificates
                    .HasForeignKey(c => c.AdminId)  // The foreign key is AdminId
                    .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete to avoid accidental deletion of Admin when Certificate is delete


            });
        }
    }
}
