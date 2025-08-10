using System.Globalization;
using MentalHealthcare.API.Extensions;
//using MentalHealthcare.API.Hubs;

//using MentalHealthcare.API.Hubs;
using MentalHealthcare.API.MiddleWares;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Extensions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Extensions;
//using MentalHealthcare.Infrastructure.NotificationService;
using MentalHealthcare.Infrastructure.Seeders;
using Microsoft.AspNetCore.Mvc;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
    builder.AddPresentation();
    builder.Services.AddApplication(builder.Configuration);
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddScoped<GlobalErrorHandling>();
    builder.Services.AddScoped<RequestTimeLogging>();
    builder.Services.AddControllers();
  //  builder.Services.AddSignalR();
    //add new 
   // builder.Services.AddSingleton<NotificationConnectionService>();

    // builder.Services.AddControllers()
    //     .AddJsonOptions(options =>
    //     {
    //         options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never;
    //         options.JsonSerializerOptions.PropertyNamingPolicy = null; // Use PascalCase if required
    //     });

    // Add CORS services
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });



    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(e => e.Value!.Errors.Count > 0)
                .SelectMany(e => e.Value!.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            var result = OperationResult<string>.Failure(errors.FirstOrDefault() ?? "Validation Error");
            result.Errors = errors;

            return new BadRequestObjectResult(result);
        };
    });






    var app = builder.Build();
    var scope = app.Services.CreateScope();
    var serviceProvider = scope.ServiceProvider.GetRequiredService<IAdminSeeder>();
    await serviceProvider.seed();
//add localization 
    app.UseRequestLocalization(options =>
    {
        var supportedCultures = new[] { "en", "ar" };
        options.SetDefaultCulture("ar")
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);
    });
    CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("ar");
    CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("ar");

// Use middlewares
    app.UseMiddleware<GlobalErrorHandling>();
    app.UseMiddleware<RequestTimeLogging>();

// Configure the HTTP request pipeline.
    app.UseHttpsRedirection();
    app.UseRouting();

// Use CORS middleware
    app.UseCors("AllowAll");

    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    //if (app.Environment.IsDevelopment())
    //{
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/Dashboard/swagger.json", "Dashboard API v1");
            c.SwaggerEndpoint("/swagger/MobileApp/swagger.json", "Mobile App API v1");
            c.SwaggerEndpoint("/swagger/Development/swagger.json", "Development API v1"); // Ensure this is included
            c.SwaggerEndpoint("/swagger/All/swagger.json", "All API v1"); // Ensure this is included
        });
    //}
  //  app.MapHub<NotificationHub>("/NotificationHub");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application startup failed");
}
finally
{
    Log.CloseAndFlush();
}