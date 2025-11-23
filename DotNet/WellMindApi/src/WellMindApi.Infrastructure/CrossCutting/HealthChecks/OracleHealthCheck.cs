using Microsoft.Extensions.Diagnostics.HealthChecks;
using Oracle.ManagedDataAccess.Client;

namespace WellMindApi.Infrastructure.CrossCutting.HealthChecks;

/// <summary>
/// Health Check customizado para Oracle Database
/// </summary>
public class OracleHealthCheck : IHealthCheck
{
    private readonly string _connectionString;

    public OracleHealthCheck(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT 1 FROM DUAL";
            var result = await command.ExecuteScalarAsync(cancellationToken);

            if (result != null)
            {
                return HealthCheckResult.Healthy("Oracle database is responsive");
            }

            return HealthCheckResult.Unhealthy("Oracle database query returned null");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                "Oracle database is not responsive",
                ex);
        }
    }
}
