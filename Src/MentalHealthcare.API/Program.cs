using MentalHealthcare.API.Extensions;
using MentalHealthcare.API.MiddleWares;
using MentalHealthcare.Application.Extensions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Extensions;
using MentalHealthcare.Infrastructure.Persistence;
using MentalHealthcare.Infrastructure.Repositories;
using MentalHealthcare.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
    builder.AddPresentation();
    builder.Services.AddApplication(builder.Configuration);
    builder.Services.AddInfrastructure(builder.Configuration);
    // Add SQL Server database context
    builder.Services.AddDbContext<MentalHealthDbContext>(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        options.UseSqlServer(connectionString);
    });

    // Register repositories
    builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();

    builder.Services.AddScoped<GlobalErrorHandling>();
    builder.Services.AddScoped<RequestTimeLogging>();
    builder.Services.AddControllers();
    //builder.Services.AddScoped<IAdminSeeder, AdminSeeder>();

    //builder.Services.AddControllers()
    //    .AddJsonOptions(options =>
    //    {
    //        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    //    });

    // Add CORS services
    //builder.Services.AddCors(options =>
    //{
    //    options.AddPolicy("AllowAll", policy =>
    //    {
    //        policy.AllowAnyOrigin()
    //            .AllowAnyMethod()
    //            .AllowAnyHeader();
    //    });
    //});

    var app = builder.Build();
    var scope = app.Services.CreateScope();
    var serviceProvider = scope.ServiceProvider.GetRequiredService<IAdminSeeder>();
    await serviceProvider.seed();

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

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseDeveloperExceptionPage();
    }
    
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