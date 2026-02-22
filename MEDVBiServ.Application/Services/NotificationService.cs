using MEDVBiServ.Application.Dtos;
using MEDVBiServ.Application.Interfaces;
using MEDVBiServ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDVBiServ.Application.Services
{
    public sealed class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;

        public NotificationService(INotificationRepository repo)
        {
            _repo = repo;
        }

        public async Task<IReadOnlyList<NotificationDto>> GetLatestAsync(int take, CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 500);

            var items = await _repo.GetLatestAsync(take, ct);

            return items.Select(x => new NotificationDto
            {
                Id = x.Id,
                Author = x.Author,
                Text = x.Text,
                CreatedAt = x.CreatedAt
            }).ToList();
        }

        public async Task<NotificationDto> CreateAsync(CreateNotificationRequest request, CancellationToken ct = default)
        {
            var author = string.IsNullOrWhiteSpace(request.Author) ? "MEDV" : request.Author.Trim();
            var text = (request.Text ?? "").Trim();

            if (text.Length == 0)
                throw new ArgumentException("Text darf nicht leer sein.");

            if (text.Length > 1000)
                throw new ArgumentException("Text darf max. 1000 Zeichen haben.");

            var now = DateTimeOffset.UtcNow;

            var entity = new NotificationMessage
            {
                Id = Guid.NewGuid(),
                Author = author.Length > 60 ? author[..60] : author,
                Text = text,
                CreatedAt = now,
                CreatedAtUnixMs = now.ToUnixTimeMilliseconds()
            };

            await _repo.AddAsync(entity, ct);

            return new NotificationDto
            {
                Id = entity.Id,
                Author = entity.Author,
                Text = entity.Text,
                CreatedAt = entity.CreatedAt
            };
        }
    }
}
