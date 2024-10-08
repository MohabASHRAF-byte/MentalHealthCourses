using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Seeders;

public class AdminSeeder(
    MentalHealthDbContext dbContext
) : IAdminSeeder
{
    public async Task seed()
    {
        if (!await dbContext.Database.CanConnectAsync())
            return;
        if (await dbContext.Users.AnyAsync())
            return;

        var adminIdentity = new User
        {
            Email = "admin@admin.com",
            NormalizedEmail = "admin@admin.com".ToUpper(),
            Roles = 1,
            Tenant = Global.ProgramName,
            PhoneNumber = "0111111111111",
            UserName = "admin",
            EmailConfirmed = true,
            LockoutEnabled = false,
            PasswordHash = "sdfdsa",
            TwoFactorEnabled = false,
            AccessFailedCount = 0,
            NormalizedUserName = "admin".ToUpper(),
            PhoneNumberConfirmed = true,
        };
        var admin = new Admin
        {
            AdminId = 1,
            User = adminIdentity,
            FName = "admin",
            LName = "admin"
        };   
        var adminIdentity1 = new User
        {
            Email = "admin1@admin.com",
            NormalizedEmail = "admin1@admin.com".ToUpper(),
            Roles = 1,
            Tenant = Global.ProgramName,
            PhoneNumber = "022222222222",
            UserName = "admin1",
            EmailConfirmed = true,
            LockoutEnabled = false,
            PasswordHash = "sdfdsa",
            TwoFactorEnabled = false,
            AccessFailedCount = 0,
            NormalizedUserName = "admin1".ToUpper(),
            PhoneNumberConfirmed = true,
        };
        var admin1 = new Admin
        {
            AdminId = 2,
            User = adminIdentity1,
            FName = "admin1",
            LName = "admin1"
        };
        await dbContext.AddAsync(adminIdentity);
        await dbContext.AddAsync(adminIdentity1);
        await dbContext.AddAsync(admin);
        await dbContext.AddAsync(admin1);
        await dbContext.SaveChangesAsync();
    }
    
}