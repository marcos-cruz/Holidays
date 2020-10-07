using Bigai.Holidays.Core.Services.Api.Configurations.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            //
            // To avoid the MultiPartBodyLength error because size of files...
            //
            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MemoryBufferThreshold = int.MaxValue;
            });

            return services;
        }

        /// <summary>
        /// Configure the request pipeline of application.
        /// </summary>
        /// <param name="app">Configure the application pipeline.</param>
        /// <param name="env">Information about the application environment.</param>
        /// <param name="provider">Defines the behavior of a provider that discovers and describes API version information within an application.</param>
        /// <returns>The same application so that multiple calls can be chained.</returns>
        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseCors("Development");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseCors("Production");
                app.UseHsts();
            }

            app.UseSwaggerConfiguration(provider);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

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
