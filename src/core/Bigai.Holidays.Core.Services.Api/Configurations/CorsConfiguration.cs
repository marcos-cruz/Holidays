using Microsoft.Extensions.DependencyInjection;

namespace Bigai.Holidays.Core.Services.Api.Configurations
{
    /// <summary>
    /// <see cref="CorsConfiguration"/> represents the settings of Cors.
    /// </summary>
    public static class CorsConfiguration
    {
        /// <summary>
        /// Adds the Cors configuration to the service collection.
        /// </summary>
        /// <param name="services">Collection of services to add the Cors configuration.</param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(cors =>
            {
                cors.AddPolicy("Development", builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());

                cors.AddPolicy("Production", builder => builder
                    //.WithMethods("GET", "PUT")
                    .WithOrigins("https://www.bigai.com.br", "https://holiday.bigai.com.br")
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    //.WithHeaders(HeaderNames.ContentType, "x-custom-header")
                    .AllowAnyHeader());
            });

            return services;
        }
    }
}
