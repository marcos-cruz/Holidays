using Bigai.Holidays.Core.Infra.Data.Contexts;
using Bigai.Holidays.Core.Services.Api.Configurations.Health.Garbage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Bigai.Holidays.Core.Services.Api.Configurations.Health
{
    /// <summary>
    /// <see cref="HealthChecksConfiguration"/> represents the settings for checking the health of the API.
    /// </summary>
    public static class HealthChecksConfiguration
    {
        /// <summary>
        /// Adds the health checks configuration to the service collection.
        /// </summary>
        /// <param name="services">Collection of services to add the API configuration.</param>
        /// <param name="configuration">Required to access the <c>appsettings.json</c> configuration file.</param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddHealthChecksConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddHealthChecks().AddGCInfoCheck("GCInfo");

            var connectionString = configuration.GetConnectionString(HolidaysContext.KeyConnectionString);

            //classe abstrata garbage collecion
            //    limpar código

            services.AddHealthChecks()
                //
                // Checks the memory allocated by the application
                //
                .AddProcessAllocatedMemoryHealthCheck(512)
                //
                // Checks the application's garbage collector
                //
                .AddGCInfoCheck("Garbage Collector")
                //
                // Checks disk space.
                //
                .AddDiskStorageHealthCheck(storage => storage.AddDrive("C:\\", 1024))
                //
                // Checks whether the database is responding
                //
                .AddSqlServer(connectionString: connectionString, healthQuery: "SELECT 1;", name: "SQL Server", failureStatus: HealthStatus.Degraded)
                //
                // Checks whether the countries table has records
                //
                .AddCheck("Countries", new SqlServerHealthCheck(connectionString));
                //
                // .AddSqlServer(connectionString, name: "SQL Server") // Checks whether the database is responding
                //
                // .AddCheck("SQL Server", new SqlConnectionHealthCheck(connectionString)) // Checks whether the database is responding
                //
                // .AddUrlGroup(new Uri("https://www.google.com"), name: "Google", failureStatus: HealthStatus.Degraded)

            services.AddHealthChecksUI();

            return services;
        }
    }
}
