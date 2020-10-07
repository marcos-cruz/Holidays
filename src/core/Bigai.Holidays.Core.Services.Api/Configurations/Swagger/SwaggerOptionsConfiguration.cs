using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;

namespace Bigai.Holidays.Core.Services.Api.Configurations.Swagger
{
    /// <summary>
    /// <see cref="SwaggerOptionsConfiguration"/> provides support for extending Swagger configuration options.
    /// </summary>
    public class SwaggerOptionsConfiguration : IConfigureOptions<SwaggerGenOptions>
    {
        #region Private Variables

        private readonly IApiVersionDescriptionProvider _provider;

        #endregion

        #region Constructor

        /// <summary>
        /// Return a instance of <see cref="SwaggerOptionsConfiguration"/>
        /// </summary>
        /// <param name="provider">Defines the behavior of a provider that discovers and describes API version information within an application.</param>
        public SwaggerOptionsConfiguration(IApiVersionDescriptionProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Obtains all versions of the API, and adds documentation for each version.
        /// </summary>
        /// <param name="options">Swagger options generator.</param>
        public void Configure(SwaggerGenOptions options)
        {
            foreach (ApiVersionDescription description in _provider.ApiVersionDescriptions)
            {
                string applicationPath = PlatformServices.Default.Application.ApplicationBasePath;
                string applicationName = PlatformServices.Default.Application.ApplicationName;
                string xmlDocPath = Path.Combine(applicationPath, $"{applicationName}.xml");

                options.IncludeXmlComments(xmlDocPath);
                options.SwaggerDoc(description.GroupName, GetOpenApiInfo(description));
            }
        }

        #endregion

        #region Private Methods

        private OpenApiInfo GetOpenApiInfo(ApiVersionDescription description)
        {
            OpenApiInfo apiInfo = new OpenApiInfo()
            {
                Title = "Holidays API",
                Description = "Microservice to search for country holidays.",
                Version = description.GroupName,
                TermsOfService = new Uri("https://holidays.bigai.com.br/terms-of-service"),
                Contact = new OpenApiContact()
                {
                    Name = "Bigai Holidays",
                    Url = new Uri("https://bigai.com.br/developers/contact"),
                    Email = "developers@bigai.com.br"
                },
                License = new OpenApiLicense()
                {
                    Name = "Bigai Comercial",
                    Url = new Uri("https://bigai.com.br/developers/licence")
                }
            };

            if (description.IsDeprecated)
            {
                apiInfo.Description += " This version is obsolete.";
            }

            return apiInfo;
        }

        #endregion
    }
}
