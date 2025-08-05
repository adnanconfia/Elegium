using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Chat
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public virtual ApplicationUser Sender { get; set; }
        public string SenderId { get; set; }
        public virtual Thread Thread { get; set; }
        public Guid? ThreadId { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public bool Deleted { get; set; }
    }
}
