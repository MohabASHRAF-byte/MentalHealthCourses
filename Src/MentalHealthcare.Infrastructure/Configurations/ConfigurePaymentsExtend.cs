using MentalHealthcare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Configurations;

public static class ConfigurePaymentsExtend
{
    public static void ConfigurePayments(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Payments>(entity =>
        {
            entity.HasKey(p => p.PaymentId);
            entity.Property(p => p.PaymentId).UseIdentityColumn(1, 1);
        });
    }
}