using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

namespace Bigai.Holidays.Core.Services.Api.Configurations.Swagger
{
    /// <summary>
    /// <see cref="SwaggerConfiguration"/> provides support for configuring Swagger and exposing API documentation.
    /// </summary>
    public static class SwaggerConfiguration
    {
        /// <summary>
        /// Adds the Swagger provider to the service collection.
        /// </summary>
        /// <param name="services">Collection of services to add the swagger configuration.</param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerDefaultValues>();

                /*
                 * 
                OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Description = "Insert the JWT Authorization Token like this: Bearer {token}",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                };

                OpenApiSecurityRequirement securityRequirements = new OpenApiSecurityRequirement()
                {
                    { securityDefinition, new string[] { }},
                };

                options.AddSecurityDefinition("Bearer", securityDefinition);

                options.AddSecurityRequirement(securityRequirements);
                */
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            // 
            // Habilita um Middleware para que o swagger seja acessado somente com autorização/login
            //
            //app.UseMiddleware<SwaggerAuthorizedMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });

            return app;
        }
    }
}
