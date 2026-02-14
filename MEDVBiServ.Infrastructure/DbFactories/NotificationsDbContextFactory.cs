using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace MEDVBiServ.Infrastructure.DbContext;

public sealed class NotificationsDbContextFactory : IDesignTimeDbContextFactory<NotificationsDbContext>
{
    public NotificationsDbContext CreateDbContext(string[] args)
    {
        // Wichtig: BasePath auf das API-Projekt setzen (damit appsettings gefunden wird)
        // Pfad ggf. anpassen, falls dein Infrastructure-Projekt anders liegt.
        var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "MEDVBiServ.API"));

        var config = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var cs = config.GetConnectionString("Notifications");
        if (string.IsNullOrWhiteSpace(cs))
            throw new InvalidOperationException("ConnectionString 'Notifications' fehlt in appsettings.");

        var optionsBuilder = new DbContextOptionsBuilder<NotificationsDbContext>();
        optionsBuilder.UseSqlite(cs);

        return new NotificationsDbContext(optionsBuilder.Options);
    }
}