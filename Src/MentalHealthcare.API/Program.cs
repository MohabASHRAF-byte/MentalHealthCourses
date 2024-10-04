using MentalHealthcare.API.Extensions;
using MentalHealthcare.API.MiddleWares;
using MentalHealthcare.Application.Extensions;
using MentalHealthcare.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddPresentation();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<GlobalErrorHandling>();
builder.Services.AddScoped<RequestTimeLogging>();
builder.Services.AddControllers();

var app = builder.Build();
app.UseMiddleware<GlobalErrorHandling>();
app.UseMiddleware<RequestTimeLogging>();
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();