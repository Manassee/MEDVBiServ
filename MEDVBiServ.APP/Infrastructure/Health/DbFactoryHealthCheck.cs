using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MEDVBiServ.App.Infrastructure.Health
{
    public sealed class DbFactoryHealthCheck<TContext> : IHealthCheck
        where TContext : Microsoft.EntityFrameworkCore.DbContext // <<< WICHTIG
    {
        private readonly IDbContextFactory<TContext> _factory;

        public DbFactoryHealthCheck(IDbContextFactory<TContext> factory)
            => _factory = factory;

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken ct = default)
        {
            try
            {
                await using var db = await _factory.CreateDbContextAsync(ct); // IAsyncDisposable ok
                var ok = await db.Database.CanConnectAsync(ct);               // Database vorhanden
                return ok ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy("CanConnect=false");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Exception", ex);
            }
        }
    }
}

