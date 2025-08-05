using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Chat
{
    public class ChatBoxes
    {
        public Guid Id { get; set; }
        public virtual ApplicationUser User{ get; set; }
        public virtual Thread Thread { get; set; }
        public string UserId { get; set; }
        public Guid? ThreadId { get; set; }
        public virtual ApplicationUser Receiver { get; set; }
        public string ReceiverId { get; set; }

    }
}
