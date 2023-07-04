using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.Extensions.Configuration;

namespace WalletWatch.WebAPI.HealthChecks
{
    public class SqlHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;

        public SqlHealthCheck(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var sqlConnection = new SqlConnection(_connectionString);

                await sqlConnection.OpenAsync(cancellationToken);

                using var command = sqlConnection.CreateCommand();
                command.CommandText = "SELECT 1";
                await command.ExecuteScalarAsync(cancellationToken);

                return new HealthCheckResult(
                HealthStatus.Healthy,
                description: "Reports Healthy status of database",
                exception: null);
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"{context.Registration.FailureStatus}", exception: ex);
            }
        }
    }
}
