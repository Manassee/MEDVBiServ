using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MEDVBiServ.Application.Interfaces;
using MEDVBiServ.Infrastructure.DbContext;
using MEDVBiServ.Infrastructure.Repository;

namespace MEDVBiServ.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment env)
        {
            var csDe = RequireCs(configuration, "BibleDe");
            var csFr = RequireCs(configuration, "BibleFr");
            var csNotif = RequireCs(configuration, "Notifications");

            csDe = NormalizeSqlitePath(csDe, env.ContentRootPath);
            csFr = NormalizeSqlitePath(csFr, env.ContentRootPath);
            csNotif = NormalizeSqlitePath(csNotif, env.ContentRootPath);

            services.AddDbContext<DEDbContext>(options => options.UseSqlite(csDe));
            services.AddDbContext<FRDbContext>(options => options.UseSqlite(csFr));
            services.AddDbContext<NotificationsDbContext>(options => options.UseSqlite(csNotif));

            services.AddScoped<IBibleVerseRepository, BibleRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            return services;
        }

        private static string RequireCs(IConfiguration cfg, string name)
            => cfg.GetConnectionString(name)
               ?? throw new InvalidOperationException($"ConnectionString '{name}' fehlt. Prüfe appsettings.json oder user-secrets.");

        /// <summary>
        /// Macht Data Source absolut (bezogen auf API-ContentRoot) und erstellt Ordner falls nötig.
        /// </summary>
        private static string NormalizeSqlitePath(string connectionString, string contentRootPath)
        {
            var builder = new SqliteConnectionStringBuilder(connectionString);

            // DataSource kann relativ oder absolut sein
            var dataSource = builder.DataSource;

            if (!Path.IsPathRooted(dataSource))
                dataSource = Path.GetFullPath(Path.Combine(contentRootPath, dataSource));

            var dir = Path.GetDirectoryName(dataSource);
            if (!string.IsNullOrWhiteSpace(dir))
                Directory.CreateDirectory(dir); // <-- verhindert Error 14

            builder.DataSource = dataSource;
            return builder.ToString();
        }
    }
}