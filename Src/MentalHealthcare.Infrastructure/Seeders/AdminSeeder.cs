using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities.Courses;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Infrastructure.Persistence;
using MentalHealthcare.Infrastructure.Seeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        // Seed Admins
        if (!dbContext.Admins.Any())
        {
            await dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Admins ON");

            var passwordHasher = new PasswordHasher<User>();

            var adminIdentity = new User
            {
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                Roles = 1,
                Tenant = Global.ProgramName,
                PhoneNumber = "0111111111111",
                UserName = "admin",
                PasswordHash = passwordHasher.HashPassword(null, "test2"),
                EmailConfirmed = true,
                NormalizedUserName = "ADMIN",
                PhoneNumberConfirmed = true,
            };
            var admin = new Admin
            {
                User = adminIdentity,
                FName = "admin",
                LName = "admin"
            };

            await dbContext.Admins.AddAsync(admin);
            await dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Admins OFF");
        }

        // Seed Categories
        if (!dbContext.Categories.Any())
        {
            var categories = new List<Category>
            {
                new() { Name = "Dev", Description = "Development" },
                new() { Name = "Think", Description = "Thinking" }
            };
            await dbContext.Categories.AddRangeAsync(categories);
        }

        // Seed Instructors
        if (!dbContext.Instructors.Any())
        {
            var admin = await dbContext.Admins.FirstOrDefaultAsync(); // Use existing Admin
            var instructors = new List<Instructor>
            {
                new() { Name = "John Doe", About = "John Doe", AddedBy = admin },
                new() { Name = "Jane Doe", About = "Another Instructor", AddedBy = admin }
            };
            await dbContext.Instructors.AddRangeAsync(instructors);
        }

        // Seed Courses
        if (!dbContext.Courses.Any())
        {
            var categories = await dbContext.Categories.ToListAsync();
            var instructor = await dbContext.Instructors.FirstOrDefaultAsync();

            var course = new Course
            {
                Name = "DSP",
                Description = "Course description",
                Categories = categories,
                Instructor = instructor,
                Price = 100,
                Rating = 4.5M,
                ThumbnailUrl = "example.com/image.jpg",
            };
            await dbContext.Courses.AddAsync(course);
        }

        // Seed Advertisements
        if (!dbContext.Advertisements.Any())
        {
            var ads = new List<Advertisement>
            {
                new()
                {
                    AdvertisementName = "Ad 1",
                    AdvertisementDescription = "First Ad",
                    IsActive = true,
                    AdvertisementImageUrls = new List<AdvertisementImageUrl>
                    {
                        new() { ImageUrl = "www.example1.com" },
                        new() { ImageUrl = "www.example2.com" }
                    }
                },
                new()
                {
                    AdvertisementName = "Ad 2",
                    AdvertisementDescription = "Second Ad",
                    IsActive = false,
                    AdvertisementImageUrls = new List<AdvertisementImageUrl>
                    {
                        new() { ImageUrl = "www.example3.com" },
                        new() { ImageUrl = "www.example4.com" }
                    }
                }
            };
            await dbContext.Advertisements.AddRangeAsync(ads);
        }






        // Seed Instructors
        if (!dbContext.Authors.Any())
        {
            var admin = await dbContext.Admins.FirstOrDefaultAsync(); // Use existing Admin
            var authors = new List<Author>
            {
                new() { Name = "John Doe", About = "John Doe", AddedBy = admin },
                new() { Name = "Jane Doe", About = "Another Instructor", AddedBy = admin }
            };
            await dbContext.Authors.AddRangeAsync(authors);
        }







        //if (!dbContext.Articles.Any())
        //{
        //    var articles = new List<Article>{
        //        new()
        //        {
        //            Title = "Ad 1",
        //            Content = "qwertyuioasdfghzxcvbnasdfghjZxcvbsdfgh",
        //            AuthorId = 1,
        //            ArticleImageUrls = new List<ArticleImageUrl>
        //            {
        //                new() { ImageUrl = "www.example1.com" },
        //                new() { ImageUrl = "www.example2.com" }
        //            } } };
        //    await dbContext.Articles.AddRangeAsync(articles);


        //}





        // Seed Authors
        //if (!dbContext.Authors.Any())
        //{
        //    var admin = await dbContext.Admins.FirstOrDefaultAsync(); // Use existing Admin
        //    var Authors = new List<Author>
        //    {
        //        new() { Name = "John Doe", About = "John Doe", AddedBy = admin },
        //        new() { Name = "Jane Doe", About = "Another Instructor", AddedBy = admin }
        //    };
        //    await dbContext.Authors.AddRangeAsync(Authors);
        //}

        await dbContext.SaveChangesAsync();
        }
    }
