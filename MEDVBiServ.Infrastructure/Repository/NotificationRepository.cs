using MEDVBiServ.Application.Interfaces;
using MEDVBiServ.Domain.Entities;
using MEDVBiServ.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDVBiServ.Infrastructure.Repository
{
    public sealed class NotificationRepository : INotificationRepository
    {
        private readonly NotificationsDbContext _db;

        public NotificationRepository(NotificationsDbContext db)
        {
            _db = db;
        }

        public async Task<IReadOnlyList<NotificationMessage>> GetLatestAsync(int take, CancellationToken ct = default)
        {
            return await _db.Notifications
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .Take(take)
                .ToListAsync(ct);
        }

        public async Task AddAsync(NotificationMessage message, CancellationToken ct = default)
        {
            _db.Notifications.Add(message);
            await _db.SaveChangesAsync(ct);
        }

    }
}
