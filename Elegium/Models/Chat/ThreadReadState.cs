using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Chat
{
    public class ThreadReadState
    {
        public Guid Id { get; set; }
        public virtual Message Message { get; set; }
        public Guid MessageId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public DateTime When { get; set; } = DateTime.UtcNow;
        public bool Read { get; set; }
        public bool Delivered { get; set; }
        public bool Deleted { get; set; }
    }
}
