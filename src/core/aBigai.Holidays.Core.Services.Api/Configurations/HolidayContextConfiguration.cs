using Bigai.Holidays.Core.Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bigai.Holidays.Core.Services.Api.Configurations
{
    /// <summary>
    /// <see cref="HolidayContextConfiguration"/> provides support for adding context to the database.
    /// </summary>
    public static class HolidayContextConfiguration
    {
        /// <summary>
        /// Adds the holidayContext provider to the service collection.
        /// </summary>
        /// <param name="services">Collection of services to add the provider context.</param>
        /// <param name="configuration">Required to access the <c>appsettings.json</c> configuration file.</param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddContextConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(HolidaysContext.KeyConnectionString);
            //services.AddDbContext<HolidaysContext>(options =>
            //{
            //    options.UseSqlServer(connectionString);
            //});

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<HolidaysContext>(
                    options => options.UseSqlServer(
                        configuration.GetConnectionString(connectionString)));


            return services;
        }
    }
}
