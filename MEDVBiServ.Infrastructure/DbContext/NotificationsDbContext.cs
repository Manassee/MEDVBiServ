using MEDVBiServ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDVBiServ.Infrastructure.DbContext
{
    public sealed class NotificationsDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public NotificationsDbContext(DbContextOptions<NotificationsDbContext> options) : base(options)
        {
        }

        public DbSet<NotificationMessage> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationMessage>(b =>
            {
                b.ToTable("notifications");
                b.HasKey(x => x.Id);

                b.Property(x => x.Author)
                    .HasMaxLength(60)
                    .IsRequired();

                b.Property(x => x.Text)
                    .HasMaxLength(1000)
                    .IsRequired();

                b.Property(x => x.CreatedAt)
                    .IsRequired();

                b.HasIndex(x => x.CreatedAt);
            });
        }
        }
}
