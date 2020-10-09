using Bigai.Holidays.Core.Services.Api.Configurations.Swagger;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });

            //
            // HealthChecks
            //
            app.UseHealthChecks("/api/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.UseHealthChecksUI(options =>
            {
                options.UIPath = "/hc-ui";
                options.AddCustomStylesheet(@"Resources\css\dotnet.css");
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                //
                // HealthChecks
                //
                /*
                 * 
                endpoints.MapHealthChecks("/api/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecksUI(options =>
                {
                    options.UIPath = "/api/hc-ui";
                    options.ResourcesPath = "/api/hc-ui-resources";

                    options.UseRelativeApiPath = false;
                    options.UseRelativeResourcesPath = false;
                    options.UseRelativeWebhookPath = false;
                });
                */
            });

            return app;
        }

        private static Task WriteResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json"; var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("results", new JObject(result.Entries.Select(pair =>
                new JProperty(pair.Key, new JObject(
                    new JProperty("status", pair.Value.Status.ToString()),
                    new JProperty("description", pair.Value.Description),
                    new JProperty("data", new JObject(pair.Value.Data.Select(p => new JProperty(p.Key, p.Value))))))))));
            return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));
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
