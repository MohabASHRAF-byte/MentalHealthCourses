using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Seeders;

public class AdminSeeder(MentalHealthDbContext dbContext) : IAdminSeeder
{
    public async Task seed()
    {
        // Apply any pending migrations
        if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
        {
            await dbContext.Database.MigrateAsync();
        }

        // Check if admins already exist to prevent duplicate entries
        if (await dbContext.Admins.AnyAsync())
        {
            return; // Exit if data already seeded
        }

        #region Seed Admins

        // Enable IDENTITY_INSERT for Admins table
        await dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Admins ON");

        // Create Admin Users
        var adminIdentity = new User
        {
            Email = "admin@admin.com",
            NormalizedEmail = "ADMIN@ADMIN.COM",
            Roles = 1,
            Tenant = Global.ProgramName,
            PhoneNumber = "0111111111111",
            UserName = "admin",
            PasswordHash = new PasswordHasher<User>().HashPassword(null, "test2"), // Secure password
            EmailConfirmed = true,
            LockoutEnabled = false,
            TwoFactorEnabled = false,
            AccessFailedCount = 0,
            NormalizedUserName = "ADMIN",
            PhoneNumberConfirmed = true,
        };

        var admin = new Admin
        {
            User = adminIdentity,
            FName = "admin",
            LName = "admin"
        };

        // Add additional admin if needed
        var adminIdentity1 = new User
        {
            Email = "admin1@admin.com",
            NormalizedEmail = "ADMIN1@ADMIN.COM",
            Roles = 1,
            Tenant = Global.ProgramName,
            PhoneNumber = "022222222222",
            UserName = "admin1",
            PasswordHash = new PasswordHasher<User>().HashPassword(null, "test2"), // Secure password
            EmailConfirmed = true,
            LockoutEnabled = false,
            TwoFactorEnabled = false,
            AccessFailedCount = 0,
            NormalizedUserName = "ADMIN1",
            PhoneNumberConfirmed = true,
        };

        var admin1 = new Admin
        {
            User = adminIdentity1,
            FName = "admin1",
            LName = "admin1"
        };

        await dbContext.AddAsync(adminIdentity);
        await dbContext.AddAsync(adminIdentity1);
        await dbContext.AddAsync(admin);
        await dbContext.AddAsync(admin1);

        await dbContext.SaveChangesAsync();

        #endregion


        #region Seed Instructors

        var instructor1 = new Instructor
        {
            Name = "John Doe",
            About = "Expert in development",
            AddedBy = admin
        };

        var instructor2 = new Instructor
        {
            Name = "Jane Smith",
            About = "Creative thinking specialist",
            AddedBy = admin
        };

        await dbContext.AddRangeAsync(instructor1, instructor2);

        #endregion

        #region Seed Courses

        var categories = await dbContext.Categories.ToListAsync();
        var course = new Course
        {
            Name = "DSP",
            Description = "Digital Signal Processing",
            Categories = categories,
            Instructor = instructor1,
            Price = 100,
            Rating = 4.5M,
            EnrollmentsCount = 10,
            IsFree = false,
            ReviewsCount = 5,
            ThumbnailUrl = "https://example.com/dsp-thumbnail.jpg",
            CollectionId = Guid.NewGuid().ToString()
        };

        await dbContext.AddAsync(course);

        #endregion

        #region Seed Advertisements

        var ad1 = new Advertisement
        {
            AdvertisementName = "Advertisement 1",
            AdvertisementDescription = "An amazing product for developers!",
            IsActive = true,
            AdvertisementImageUrls = new List<AdvertisementImageUrl>
            {
                new() { ImageUrl = "https://example.com/ad1-img1.jpg" },
                new() { ImageUrl = "https://example.com/ad1-img2.jpg" }
            }
        };

        var ad2 = new Advertisement
        {
            AdvertisementName = "Advertisement 2",
            AdvertisementDescription = "Tools to boost your productivity.",
            IsActive = false,
            AdvertisementImageUrls = new List<AdvertisementImageUrl>
            {
                new() { ImageUrl = "https://example.com/ad2-img1.jpg" },
                new() { ImageUrl = "https://example.com/ad2-img2.jpg" }
            }
        };

        await dbContext.AddRangeAsync(ad1, ad2);

        #endregion

        // Save all changes
        await dbContext.SaveChangesAsync();
    }
}
