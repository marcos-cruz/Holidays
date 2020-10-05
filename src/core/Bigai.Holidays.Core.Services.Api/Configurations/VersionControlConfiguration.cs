using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Bigai.Holidays.Core.Services.Api.Configurations
{
    /// <summary>
    /// <see cref="VersionControlConfiguration"/> provides support to standardize Api's versioning control.
    /// </summary>
    public static class VersionControlConfiguration
    {
        /// <summary>
        /// Adds Api's versioning control.
        /// </summary>
        /// <param name="services">Collection of application services to add versioning control.</param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddVersioningControlConfiguration(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }
    }
}
