using MentalHealthcare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Configurations;

public static class ConfigureSystemUserExtend
{
    public static void ConfigureSystemUser(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SystemUser>(entity =>
        {
            entity.HasKey(u => u.SystemUserId);
            entity.Property(u => u.SystemUserId)
                .UseIdentityColumn(1, 1);

            entity.Property(u => u.FName).IsRequired();
            entity.Property(u => u.LName).IsRequired();
        });
    }
}