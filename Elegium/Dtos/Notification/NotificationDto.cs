using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elegium.Models;
using Elegium.Models.Notifications;

namespace Elegium.Dtos.Notification
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public string When { get; set; }
        public string Title { get; set; }
        public string NotificationText { get; set; }
        public string SenderId { get; set; }
        public bool Read { get; set; }
        public string Url { get; set; }
    }
}
