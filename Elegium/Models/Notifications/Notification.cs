using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Notifications
{
    public class Notification
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public bool Deleted { get; set; }
        public virtual NotificationType NotificationType { get; set; }
        public Guid NotificationTypeId { get; set; }
        public string NotificationText { get; set; }
        public virtual ApplicationUser Receipient { get; set; }
        public string ReceipientId { get; set; }
        public virtual ApplicationUser Sender { get; set; }
        public string SenderId { get; set; }
        public bool Read { get; set; }
        public string Url { get; set; }
    }
}
