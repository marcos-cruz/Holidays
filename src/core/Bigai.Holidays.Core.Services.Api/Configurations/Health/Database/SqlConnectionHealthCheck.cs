using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace Bigai.Holidays.Core.Services.Api.Configurations.Health.Database
{
    public class SqlConnectionHealthCheck : DbConnectionHealthCheck
    {
        private static readonly string DefaultTestQuery = "Select 1;";

        public SqlConnectionHealthCheck(string connectionString) : this(connectionString, testQuery: DefaultTestQuery)
        {
        }

        public SqlConnectionHealthCheck(string connectionString, string testQuery) : base(connectionString, testQuery ?? DefaultTestQuery)
        {
        }

        protected override DbConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
