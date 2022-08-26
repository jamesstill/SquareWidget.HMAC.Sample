using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace PSAApi.Services
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var filePath = Path.Combine(System.AppContext.BaseDirectory, xmlFile);
            var serviceTitle = Assembly.GetExecutingAssembly().GetName().Name;
            var serviceDescription = "SampleApi";
            var openApiContact = new OpenApiContact { Name = "The A Team" };

            var openApiInfoV1 = new OpenApiInfo
            {
                Title = serviceTitle,
                Version = "v1",
                Description = serviceDescription,
                Contact = openApiContact
            };

            services.AddSwaggerGen(c =>
            {
                var securityScheme = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Authorization header. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    Type = SecuritySchemeType.OpenIdConnect
                };

                var securityRequirement = new OpenApiSecurityRequirement
                    {
                    { securityScheme, Array.Empty<string>() }
                    };

                c.AddSecurityDefinition("Bearer", securityScheme);
                c.AddSecurityRequirement(securityRequirement);

                c.SwaggerDoc("v1", openApiInfoV1);
                c.IncludeXmlComments(filePath);
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerService(this IApplicationBuilder app)
        {
            var serviceTitle = Assembly.GetExecutingAssembly().GetName().Name;
            var endpointName = serviceTitle + " v1";

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", endpointName);
            });

            return app;
        }
    }
}
