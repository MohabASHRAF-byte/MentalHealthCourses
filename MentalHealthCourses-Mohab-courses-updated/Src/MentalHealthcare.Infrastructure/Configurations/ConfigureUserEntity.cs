using MentalHealthcare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MentalHealthcare.Infrastructure.Configurations;

public static class ConfigureUserEntity
{
    public static void ConfigureUserIdentity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            #region UserName

            entity.HasIndex(u => u.UserName)
                .IsUnique(false);
            entity.HasIndex(u => u.NormalizedUserName)
                .IsUnique(false);
            
            entity.HasIndex(u => new { u.NormalizedUserName, u.Tenant })
                .IsUnique();
            entity.Property(u => u.NormalizedUserName)
                .IsRequired()
                .HasMaxLength(100);

            // UserName and Tenant combination must be unique
            entity.HasIndex(u => new { u.UserName, u.Tenant })
                .IsUnique();
            entity.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(100);


            #endregion
            
            #region Email
            entity.HasIndex(u => u.Email)
                .IsUnique(false);
            entity.HasIndex(u => u.NormalizedEmail)
                .IsUnique(false);
            entity.HasIndex(u => new { u.Email, u.Tenant })
                .IsUnique();
            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);
            entity.HasIndex(u => new { u.NormalizedEmail, u.Tenant })
                .IsUnique();
            entity.Property(u => u.NormalizedEmail)
                .IsRequired()
                .HasMaxLength(100);
            #endregion
           
            #region phone
            entity.HasIndex(u => u.PhoneNumber)
                .IsUnique(false);
            #endregion
        });
    }
}