using MentalHealthcare.API.Extensions;
using MentalHealthcare.API.MiddleWares;
using MentalHealthcare.Application.Extensions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Extensions;
using MentalHealthcare.Infrastructure.Persistence;
using MentalHealthcare.Infrastructure.Repositories;
using MentalHealthcare.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MentalHealthDbContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.AddPresentation();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<GlobalErrorHandling>();
builder.Services.AddScoped<RequestTimeLogging>();
builder.Services.AddControllers();
builder.Services.AddScoped<IAdvertisementRepository , AdvertisementRepository>();
builder.Services.AddScoped<IArticleRepository , ArticleRepository>();
builder.Services.AddScoped<IMeditationRepository , MeditationRepository>();
builder.Services.AddScoped<IPodcastRepository , PodcastRepository>();
builder.Services.AddScoped<IHelpCenterRepository, HelpCenterRepository>();

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
}

app.Run();