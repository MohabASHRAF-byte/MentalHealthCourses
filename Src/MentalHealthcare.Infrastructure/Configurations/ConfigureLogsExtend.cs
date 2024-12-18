using MentalHealthcare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Configurations;

public  static class ConfigureLogsExtend
{
    public static void ConfigureLogs(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Logs>(entity =>
        {
            entity.HasKey(l => l.Id);
            entity.Property(l => l.Id).UseIdentityColumn(1, 1);
        });
    }
}