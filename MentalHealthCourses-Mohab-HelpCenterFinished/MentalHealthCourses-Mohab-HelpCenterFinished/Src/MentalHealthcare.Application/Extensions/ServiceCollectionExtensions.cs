using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using MentalHealthcare.Application.Background_Services;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Application.Utitlites.EmailProvider;
using MentalHealthcare.Application.Utitlites.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace MentalHealthcare.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;

            services.AddAutoMapper(applicationAssembly);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
            services.AddValidatorsFromAssembly(applicationAssembly)
                .AddFluentValidationAutoValidation();

            services.AddHttpContextAccessor();
            services.ConfigureBunney(configuration);
            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<BunnyClient>();
            services.AddSingleton<IHostedService, OtpService>();
            services.AddSingleton<OtpService>(); // Register it as a service that can be injected
            services.AddScoped<IEmailSender, EmailProvider>();
            services.AddScoped<IJwtToken, JwtToken>();
            services.AddScoped<IUserContext, UserContext>();
            //  
            services.ConfigureAuthentication(configuration);
            //
            services.AddAuthorization(); // Add Authorization globally
        }

        private static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    ClockSkew = TimeSpan.FromMinutes(5) // Adjusted clock skew
                };
            });
        }

        private static void ConfigureBunney(this IServiceCollection services, IConfiguration configuration)
        {
        }
    }
}