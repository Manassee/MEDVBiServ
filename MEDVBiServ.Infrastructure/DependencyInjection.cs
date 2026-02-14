
using MEDVBiServ.Infrastructure.Repository;
using MEDVBiServ.Infrastructure.DbContext;   // Namespace anpassen
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MEDVBiServ.Application.Interfaces;

namespace MEDVBiServ.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // -----------------------------
            // ConnectionStrings laden
            // -----------------------------
            var csDe = configuration.GetConnectionString("BibleDe");
            var csFr = configuration.GetConnectionString("BibleFr");

            if (string.IsNullOrWhiteSpace(csDe))
                throw new InvalidOperationException(
                    "ConnectionString 'BibleDe' fehlt. Prüfe appsettings.json oder user-secrets.");

            if (string.IsNullOrWhiteSpace(csFr))
                throw new InvalidOperationException(
                    "ConnectionString 'BibleFr' fehlt. Prüfe appsettings.json oder user-secrets.");

            // -----------------------------
            // DbContexts registrieren
            // -----------------------------
            services.AddDbContext<DEDbContext>(options =>
                options.UseSqlite(csDe));

            services.AddDbContext<FRDbContext>(options =>
                options.UseSqlite(csFr));

            services.AddDbContext<NotificationsDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("Notifications")));

            // -----------------------------
            // Repositories
            // -----------------------------
            services.AddScoped<IBibleVerseRepository, BibleRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            return services;
        }
    }
}
