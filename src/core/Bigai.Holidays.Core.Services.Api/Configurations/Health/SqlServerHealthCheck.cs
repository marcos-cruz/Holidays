using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Services.Api.Configurations.Health
{
    /// <summary>
    /// <see cref="SqlServerHealthCheck"/> contains methods to check the health of the database.
    /// </summary>
    public class SqlServerHealthCheck : IHealthCheck
    {
        #region Private Variables

        readonly string _connection;

        #endregion

        #region Constructor

        /// <summary>
        /// Return a instance of <see cref="SqlServerHealthCheck"/>.
        /// </summary>
        /// <param name="connection">Connection string to database.</param>
        public SqlServerHealthCheck(string connection)
        {
            _connection = connection;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Perform a query to check how many countries are registered in the Countries table.
        /// </summary>
        /// <param name="context">Represent the context information associated.</param>
        /// <param name="cancellationToken">Notification that operations should be canceled.</param>
        /// <returns><see cref="HealthCheckResult"/>The result of a health check</returns>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                using (var connection = new SqlConnection(_connection))
                {
                    await connection.OpenAsync(cancellationToken);

                    var command = connection.CreateCommand();
                    command.CommandText = "select count(id) from countries";

                    return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken)) > 0 ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy();
                }
            }
            catch (Exception)
            {
                return HealthCheckResult.Unhealthy();
            }
        }

        #endregion
    }
}
