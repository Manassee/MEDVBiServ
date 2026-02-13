using MEDVBiServ.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDVBiServ.Application.Interfaces
{
    public interface INotificationService
    {
        Task<IReadOnlyList<NotificationDto>> GetLatestAsync(int take, CancellationToken ct = default);
        Task<NotificationDto> CreateAsync(CreateNotificationRequest request, CancellationToken ct = default);
    }
}
