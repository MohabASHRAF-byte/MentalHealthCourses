using MentalHealthcare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Configurations;

public static class ConfigureAdminExtend
{
    public static void ConfigureAdmin(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(a => a.AdminId);
            entity.Property(a => a.AdminId).UseIdentityColumn(1, 1);
            entity.Property(a => a.FName).IsRequired();
            entity.Property(a => a.LName).IsRequired();
        });
    }
}