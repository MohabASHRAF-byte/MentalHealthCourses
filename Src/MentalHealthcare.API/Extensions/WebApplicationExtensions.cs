using MentalHealthcare.Domain.Constants;
using Microsoft.OpenApi.Models;
using Serilog;

namespace MentalHealthcare.API.Extensions;

public static class WebApplicationExtensions
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer"
                    }
                );
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
                        },
                        []
                    }
                });
            }
        );
        builder.Services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();

            // Define Swagger documents for Dashboard, Mobile App, and Development
            c.SwaggerDoc(Global.DashboardVersion, new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Dashboard API",
                Version = "v1"
            });
            c.SwaggerDoc(Global.AllVersion, new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "All API",
                Version = "v1"
            });

            c.SwaggerDoc(Global.MobileVersion, new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Mobile App API",
                Version = "v1"
            });

            c.SwaggerDoc(Global.DevelopmentVersion, new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Development API",
                Version = "v1",
                Description = "API Endpoints for Development and Testing"
            });

            // Include endpoints in respective documents
            c.DocInclusionPredicate((docName, apiDesc) =>
            {
                if (docName == Global.DashboardVersion && 
                    (apiDesc.GroupName == Global.DashboardVersion || apiDesc.GroupName == Global.SharedVersion))
                    return true;

                if (docName == Global.MobileVersion && 
                    (apiDesc.GroupName == Global.MobileVersion || apiDesc.GroupName == Global.SharedVersion))
                    return true;

                if (docName == Global.DevelopmentVersion && 
                    apiDesc.GroupName == Global.DevelopmentVersion )
                    return true;
                if (docName == Global.AllVersion && 
                    apiDesc.GroupName == Global.AllVersion )
                    return true;

                return false;
            });
        });


        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration);
        });


    }
}