using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Chat
{
    public class Thread
    {
        public Guid ThreadId { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public virtual ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public virtual Message LastMessage { get; set; }
        public Guid? LastMessageId { get; set; }
    }
}
