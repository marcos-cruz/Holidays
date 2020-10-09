using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Collections.Generic;

namespace Bigai.Holidays.Core.Services.Api.Configurations.Health.Garbage
{
    // This is an example of a custom health check that implements IHealthCheck.
    //
    // This example also shows a technique for authoring a health check that needs to be registered
    // with additional configuration data. This technique works via named options, and is useful
    // for authoring health checks that can be disctributed as libraries.

    public static class GCInfoHealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddGCInfoCheck(
            this IHealthChecksBuilder builder,
            string name,
            HealthStatus? failureStatus = null,
            IEnumerable<string> tags = null,
            long? thresholdInBytes = null)
        {
            // Register a check of type GCInfo
            builder.AddCheck<GCInfoHealthCheck>(name, failureStatus ?? HealthStatus.Degraded, tags);

            // Configure named options to pass the threshold into the check.
            if (thresholdInBytes.HasValue)
            {
                builder.Services.Configure<GCInfoOptions>(name, options =>
                {
                    options.Threshold = thresholdInBytes.Value;
                });
            }

            return builder;
        }
    }
}
