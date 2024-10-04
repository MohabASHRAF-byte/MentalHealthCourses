using MentalHealthcare.Infrastructure.Persistence;
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

    // public static async void Seed(this IServiceProvider services)
    // {
    //     var scope = services.CreateScope();
    //     var seeder = scope.ServiceProvider.GetRequiredService<IDriverSeeder>();
    //     await seeder.Seed();
    // }
    private static void AddDataBase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<MentalHealthDbContext>(options =>
            options.UseNpgsql(connectionString)
                .EnableSensitiveDataLogging()
        );

        // services.AddScoped<IDriverSeeder, DriverSeeder>();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
   }

    private static void AddIdentity(this IServiceCollection services)
    {
        // services.AddIdentity<User, IdentityRole>()
        //     .AddEntityFrameworkStores<BookooDbContext>()
        //     .AddDefaultTokenProviders();
        // //Todo configure the password for letter 
        // services.Configure<IdentityOptions>(options =>
        // {
        //     options.Password.RequireDigit = false;
        //     options.Password.RequireLowercase = false;
        //     options.Password.RequireUppercase = false;
        //     options.Password.RequireNonAlphanumeric = false;
        //     options.Password.RequiredUniqueChars = 1;
        //     options.Password.RequiredLength = 3;
        // });
        // services.AddIdentityCore<User>()
        //     .AddRoles<IdentityRole>()
        //     .AddTokenProvider<DataProtectorTokenProvider<User>>("Bookoo")
        //     .AddEntityFrameworkStores<BookooDbContext>()
        //     .AddDefaultTokenProviders();

        // services.AddIdentityApiEndpoints<User>()
        //     .AddRoles<IdentityRole>()
        // .AddClaimsPrincipalFactory<RestaurantUserClaimsPrincipalFactory>()
        // .AddEntityFrameworkStores<BookooDbContext>();
    }
}