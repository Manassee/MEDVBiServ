using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDVBiServ.Application.Dtos
{
    public sealed class NotificationDto
    {
        public Guid Id { get; set; }
        public string Author { get; set; } = "";
        public string Text { get; set; } = "";
        public DateTimeOffset CreatedAt { get; set; }
    }

    public sealed class CreateNotificationRequest
    {
        public string Author { get; set; } = "MEDV";
        public string Text { get; set; } = "";
    }
}
