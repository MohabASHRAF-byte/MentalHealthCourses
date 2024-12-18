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
        if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
        {
            await dbContext.Database.MigrateAsync();
        }
        return;
        if (await dbContext.Advertisements.AnyAsync())
            return;
        await dbContext.Database.MigrateAsync();

        #region Seed Admins

        await dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Admins ON");
        // Create a PasswordHasher instance
        var adminIdentity = new User
        {
            Email = "admin@admin.com",
            NormalizedEmail = "admin@admin.com".ToUpper(),
            Roles = 1,
            Tenant = Global.ProgramName,
            PhoneNumber = "0111111111111",
            UserName = "admin",
            PasswordHash = "AQAAAAIAAYagAAAAEKjlm5QQZA9XA8FRFwwh/i57/vjMOj/Zuom12UHxYXW1G71RA8ytLh6CJAK5lBZQHA==",
            //password = "test2"
            EmailConfirmed = true,
            LockoutEnabled = false,
            TwoFactorEnabled = false,
            AccessFailedCount = 0,
            NormalizedUserName = "admin".ToUpper(),
            PhoneNumberConfirmed = true,
        };

        var admin = new Admin
        {
            User = adminIdentity,
            FName = "admin",
            LName = "admin"
        };
        await dbContext.PendingAdmins.AddAsync(new()
        {
            Admin = admin,
            Email = "wowopa7014@jonespal.com",
        });
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
            PasswordHash = "AQAAAAIAAYagAAAAEKjlm5QQZA9XA8FRFwwh/i57/vjMOj/Zuom12UHxYXW1G71RA8ytLh6CJAK5lBZQHA==",
            //password = "test2"
            TwoFactorEnabled = false,
            AccessFailedCount = 0,
            NormalizedUserName = "admin1".ToUpper(),
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

        #region Seed Categories

        var cat1 = new Category()
        {
            Name = "Dev",
            Description = "Development",
        };
        var cat2 = new Category
        {
            Name = "Think",
            Description = "Thinking",
        };
        await dbContext.AddAsync(cat1);
        await dbContext.AddAsync(cat2);
        await dbContext.SaveChangesAsync();

        #endregion

        #region Seed instructor

        var instructor1 = new Instructor
        {
            Name = "John Doe",
            About = "John Doe",
            AddedBy = admin,
        };
        var instructor2 = new Instructor
        {
            Name = "John Doe 2",
            About = "sadf",
            AddedBy = admin
        };

        #endregion

        #region Seed Courses

        var course = new Course()
        {
            Name = "DSP",
            Description = "cous",
            Categories = new List<Category> { cat1, cat2 },
            Instructor = instructor1,
            Price = 100,
            Rating = 2.4M,
            EnrollmentsCount = 5,
            IsFree = false,
            ReviewsCount = 2,
            IsPublic = false,
            ThumbnailUrl = "fsadfa",
            CollectionId = "28d97e2c-2561-44a9-bb55-1cb8ed14807a",
        };
        // var Matrials = new List<CourseMateriel>()
        // {
        //     new CourseMateriel()
        //     {
        //         Admin = admin,
        //         Description = "mat 1",
        //         Url = "safsadfasf.com",
        //         Title = "Mat 1",
        //         IsVideo = true,
        //         ItemOrder = 1,
        //         Course = course
        //     },
        //     new()
        //     {
        //         Admin = admin,
        //         Description = "mat 2",
        //         Url = "safsadfasf.com2",
        //         Title = "Mat 2",
        //         IsVideo = true,
        //         ItemOrder = 2,
        //         Course = course
        //
        //     }
        // };
        //

        #endregion

        #region Add Advertisement

        var ad1 = new Advertisement()
        {
            AdvertisementName = "Advertisement 1",
            AdvertisementDescription = "Advertisement description",
            IsActive = true,
        };
        var uploadedImages = new List<AdvertisementImageUrl>
        {
            new() { ImageUrl = "www.example1.com", Advertisement = ad1 },
            new() { ImageUrl = "www.example2.com", Advertisement = ad1 },
            new() { ImageUrl = "www.example3.com", Advertisement = ad1 },
        };
        ad1.AdvertisementImageUrls = uploadedImages;

        var ad2 = new Advertisement()
        {
            AdvertisementName = "Advertisement 2",
            AdvertisementDescription = "Advertisement description",
            IsActive = false,
        };
        var uploadedImages2 = new List<AdvertisementImageUrl>
        {
            new() { ImageUrl = "www.example1.com", Advertisement = ad2 },
            new() { ImageUrl = "www.example2.com", Advertisement = ad2 },
            new() { ImageUrl = "www.example3.com", Advertisement = ad2 },
        };
        ad2.AdvertisementImageUrls = uploadedImages2;

        #endregion


        await dbContext.AddAsync(instructor1);
        await dbContext.AddAsync(instructor2);
        // await dbContext.AddRangeAsync(Matrials);
        await dbContext.AddAsync(course);
        await dbContext.AddAsync(ad1);
        await dbContext.AddAsync(ad2);
        await dbContext.SaveChangesAsync();
    }
}