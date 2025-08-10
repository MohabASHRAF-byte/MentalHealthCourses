using AngleSharp.Dom;
using MentalHealthcare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Infrastructure.Configurations
{
    public static class ConfigureNotificationExtend
    {
        public static void ConfigureNotification(this ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notifications");
                entity.HasKey(n => n.Id);
                entity.Property(n => n.Title)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(n => n.Message)
                  .IsRequired()
                  .HasMaxLength(1000);

            entity.Property(n => n.CreatedAt)
            .IsRequired();

            entity.Property(n => n.IsRead)
                  .IsRequired();

            entity.Property(n => n.UserId)
                  .IsRequired();

                // Optional: If you have a relationship with a User entity
                entity.HasOne(n => n.User);
         modelBuilder.Entity<Notification>()
    .HasOne(n => n.User)
    .WithMany(u => u.Notifications)
    .HasForeignKey(n => n.UserId)
    .HasPrincipalKey(u => u.SystemUserId);

            });








        }
}
}

