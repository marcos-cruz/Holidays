using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Bigai.Holidays.Core.Services.Api.Configurations
{
    /// <summary>
    /// <see cref="ApiConfiguration"/> represents the settings of the api.
    /// </summary>
    public static class ApiConfiguration
    {
        /// <summary>
        /// Adds the api configuration to the service collection.
        /// </summary>
        /// <param name="services">Collection of services to add the API configuration.</param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddVersioningControlConfiguration();

            services.DisableModelStateConfiguration();

            services.AddCorsConfiguration();

            return services;
        }

        /// <summary>
        /// Configure the request pipeline of application.
        /// </summary>
        /// <param name="app">Mechanisms to configure an application's request.</param>
        /// <returns>The same application so that multiple calls can be chained.</returns>
        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app)
        {

            return app;
        }

        /// <summary>
        /// Disables model state validation so that you can standardize error messages.
        /// </summary>
        /// <param name="services">Collection of services to add the disable model state.</param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        private static IServiceCollection DisableModelStateConfiguration(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            return services;
        }
    }
}
