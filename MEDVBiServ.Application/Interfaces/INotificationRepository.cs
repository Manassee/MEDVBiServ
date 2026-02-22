using MEDVBiServ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDVBiServ.Application.Interfaces
{
    public interface INotificationRepository
    {
        Task<IReadOnlyList<NotificationMessage>> GetLatestAsync(int take, CancellationToken ct = default);
        Task AddAsync(NotificationMessage message, CancellationToken ct = default);

    }
}
