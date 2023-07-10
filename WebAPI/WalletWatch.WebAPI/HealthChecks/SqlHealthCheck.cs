using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MySqlConnector;

namespace WalletWatch.WebAPI.HealthChecks
{
    public class SqlHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;
        private readonly IHostEnvironment _env;

        public SqlHealthCheck(IConfiguration configuration, IHostEnvironment env)
        {
            _env = env;
            if (_env.IsDevelopment())
            {
                _connectionString = configuration.GetConnectionString("DefaultConnection");
            }
            else
            {
                _connectionString = configuration["DefaultConnection"];
            }
        }
        
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if(_env.IsDevelopment()) 
                {
                    await CheckHealthDev(cancellationToken);
                }
                else
                {
                    await CheckHealthProd(cancellationToken);
                }

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

        private async Task CheckHealthDev(CancellationToken cancellationToken = default)
        {
            using var sqlConnection = new SqlConnection(_connectionString);

            await sqlConnection.OpenAsync(cancellationToken);

            using var command = sqlConnection.CreateCommand();
            command.CommandText = "SELECT 1";
            await command.ExecuteScalarAsync(cancellationToken);
        }
        private async Task CheckHealthProd(CancellationToken cancellationToken = default)
        {
            using var sqlConnection = new MySqlConnection(_connectionString);

            await sqlConnection.OpenAsync(cancellationToken);

            using var command = sqlConnection.CreateCommand();
            command.CommandText = "SELECT 1";
            await command.ExecuteScalarAsync(cancellationToken);
        }
    }
}
