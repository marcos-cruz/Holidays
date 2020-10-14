using Bigai.Holidays.Core.Services.Api.Configurations;
using Bigai.Holidays.Core.Services.Api.Configurations.Health;
using Bigai.Holidays.Core.Services.Api.Configurations.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bigai.Holidays.Core.Services.Api
{
    /// <summary>
    /// <see cref="Startup"/> provides support for specifying application startup methods.
    /// </summary>
    public class Startup
    {
        #region Properties

        /// <summary>
        /// To access the configuration file.
        /// </summary>
        public IConfiguration Configuration { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Return a instance of <see cref="Startup"/>
        /// </summary>
        /// <param name="hostingEnvironment">To access the configuration file.</param>
        public Startup(IWebHostEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{ hostingEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            if (hostingEnvironment.IsProduction())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container. 
        /// </summary>
        /// <param name="services">Collection of services in this application.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddContextConfiguration(Configuration);

            services.AddApiConfiguration();

            services.AddSwaggerConfiguration();

            services.AddDependencyInjections();

            services.AddHealthChecksConfiguration(Configuration);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Configure the application pipeline.</param>
        /// <param name="env">Information about the application environment.</param>
        /// <param name="provider">Defines the behavior of a provider that discovers and describes API version information within an application.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            app.UseApiConfiguration(env, provider);
        }

        #endregion
    }
}
