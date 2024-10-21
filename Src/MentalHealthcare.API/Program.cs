using FluentValidation;
using MediatR;
using MentalHealthcare.API.Extensions;
using MentalHealthcare.API.MiddleWares;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Extensions;
using MentalHealthcare.Infrastructure.Extensions;
using MentalHealthcare.Infrastructure.Seeders;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddPresentation();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<GlobalErrorHandling>();
builder.Services.AddScoped<RequestTimeLogging>();
builder.Services.AddControllers();

// Get Validators
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
// 
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Validation_Behaviour<,>));






var app = builder.Build();
var scope = app.Services.CreateScope();
var serviceProvider = scope.ServiceProvider.GetRequiredService<IAdminSeeder>();
await serviceProvider.seed();
app.UseMiddleware<GlobalErrorHandling>();
app.UseMiddleware<RequestTimeLogging>();
// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();