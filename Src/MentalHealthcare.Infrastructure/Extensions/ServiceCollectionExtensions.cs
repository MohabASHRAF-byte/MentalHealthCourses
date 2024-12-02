using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Persistence;
using MentalHealthcare.Infrastructure.Repositories;
using MentalHealthcare.Infrastructure.Seeders;
using MentalHealthcare.Infrastructure.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MentalHealthcare.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataBase(configuration);
        services.AddRepositories();
        services.AddIdentity();
        
    }

    public static async void Seed(this IServiceProvider services)
    {
        var scope = services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<IAdminSeeder>();
        await seeder.seed();
    }
    private static void AddDataBase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<MentalHealthDbContext>(options =>
            options.UseNpgsql(connectionString)
                .EnableSensitiveDataLogging()
        );
        services.AddScoped<IUserValidator<User>, RegisterUserUserValidator<User>>();

    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAdminRepository, AdminRepository>();
        services.AddScoped<IAdminSeeder, AdminSeeder>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IHelpCenterRepository, HelpCenterRepository>();
        services.AddScoped<IAdvertisementRepository, AdvertisementRepository>();
        services.AddScoped<IContactUsRepository, ContactUsRepository>();
    }

    private static void AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<MentalHealthDbContext>()
            .AddDefaultTokenProviders();
        //Todo configure the password for letter 
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredUniqueChars = 1;
            options.Password.RequiredLength = 3;
        });
    }
}