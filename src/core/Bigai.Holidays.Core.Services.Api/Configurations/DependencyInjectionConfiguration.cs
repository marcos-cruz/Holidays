using Bigai.Holidays.Core.Infra.CrossCutting.IoC;
using Bigai.Holidays.Core.Services.Api.Configurations.Swagger;
using Bigai.Holidays.Shared.Infra.CrossCutting.Interfaces;
using Bigai.Holidays.Shared.Infra.CrossCutting.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Bigai.Holidays.Core.Services.Api.Configurations
{
    /// <summary>
    /// <see cref="DependencyInjectionConfiguration"/> provides support for dependency injection resolution.
    /// </summary>
    public static class DependencyInjectionConfiguration
    {
        /// <summary>
        /// Adds all dependency injections to the service collection.
        /// </summary>
        /// <param name="services">Collection of application services to add the dependency injections.</param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddDependencyInjections(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserLogged, UserLogged>();
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerOptionsConfiguration>();

            HolidayDependencyInjection.RegisterDependencies(services);

            return services;
        }
    }
}
