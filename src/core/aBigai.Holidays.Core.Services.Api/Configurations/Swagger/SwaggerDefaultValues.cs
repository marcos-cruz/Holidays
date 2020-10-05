using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Bigai.Holidays.Core.Services.Api.Configurations.Swagger
{
    /// <summary>
    /// <see cref="SwaggerDefaultValues"/> allow us to customize and control the API metadata.
    /// </summary>
    public class SwaggerDefaultValues : IOperationFilter
    {
        /// <summary>
        /// Customize and control the API metadata.
        /// </summary>
        /// <param name="operation">Operation Object.</param>
        /// <param name="context">Operation Filter Context.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            ApiDescription apiDescription = context.ApiDescription;

            operation.Deprecated = apiDescription.IsDeprecated();

            if (operation.Parameters == null)
            {
                return;
            }

            foreach (OpenApiParameter parameter in operation.Parameters.OfType<OpenApiParameter>())
            {
                ApiParameterDescription description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

                if (parameter.Description == null)
                {
                    parameter.Description = description.ModelMetadata?.Description;
                }

                //if (parameter.Default == null)
                //{
                //    parameter.Default = description.DefaultValue;
                //}

                parameter.Required |= description.IsRequired;
            }
        }
    }
}
