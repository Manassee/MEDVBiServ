using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MEDVBiServ.Infrastructure.DbContext;

public sealed class NotificationsDbContextFactory : IDesignTimeDbContextFactory<NotificationsDbContext>
{
    public NotificationsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<NotificationsDbContext>();

        // Pfad wie du willst – hier: Datei im Startup-Verzeichnis
        var connectionString = "Data Source=notifications.sqlite";

        optionsBuilder.UseSqlite(connectionString);

        return new NotificationsDbContext(optionsBuilder.Options);
    }
}