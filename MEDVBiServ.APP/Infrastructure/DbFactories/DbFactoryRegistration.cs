using Microsoft.EntityFrameworkCore;

namespace MEDVBiServ.App.Infrastructure.DbFactories
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    public static class DbFactoryRegistration
    {
        public static IServiceCollection AddSqliteDbContexts<TDe, TFr, TEn>(
            this IServiceCollection services,
            IConfiguration config)
            where TDe : DbContext
            where TFr : DbContext
            where TEn : DbContext
        {
            var csDe = Require(config.GetConnectionString("DE_Bible"), "DE_Bible");
            var csFr = Require(config.GetConnectionString("FR_Bible"), "FR_Bible");
            var csEn = Require(config.GetConnectionString("EN_Bible"), "EN_Bible");

            // 1) Gepoolte FACTORIES (für deine normalen Datenzugriffe)
            services.AddPooledDbContextFactory<TDe>(o => o.UseSqlite(csDe));
            services.AddPooledDbContextFactory<TFr>(o => o.UseSqlite(csFr));
            services.AddPooledDbContextFactory<TEn>(o => o.UseSqlite(csEn));

            // 2) Zusätzliche DbContext-REGISTRIERUNG (scoped) – nur damit AddDbContextCheck<> funktioniert
            services.AddDbContext<TDe>(o => o.UseSqlite(csDe));
            services.AddDbContext<TFr>(o => o.UseSqlite(csFr));
            services.AddDbContext<TEn>(o => o.UseSqlite(csEn));

            // 3) HealthChecks per AddDbContextCheck<TContext>()
            services.AddHealthChecks()
                .AddDbContextCheck<TDe>("DE_Bible")
                .AddDbContextCheck<TFr>("FR_Bible")
                .AddDbContextCheck<TEn>("EN_Bible");
           
            return services;

            static string Require(string? v, string name) =>
                !string.IsNullOrWhiteSpace(v) ? v
                    : throw new InvalidOperationException($"ConnectionStrings:{name} fehlt.");
        }
    }
}