using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos.Chat
{
    public class ConversationThreadsDto
    {
        public string Name { get; set; }
        public Guid? MessageId { get; set; }
        public string Text { get; set; }
        public DateTime When { get; set; }
        public bool Read { get; set; }

        [NotMapped]
        public bool Delivered { get; set; }
        public Guid? ThreadId { get; set; }

        [NotMapped]
        public  bool IsItFromMe { get; set; }
        public string UserId { get; set; }
        [NotMapped]
        public bool? Online { get; set; } = false;
        [NotMapped]
        public bool Opened { get; set; }
        [NotMapped]
        public bool Deleted { get; set; }

        [NotMapped]
        public string FriendlyTime { get; set; }
        public int UnreadMsgs { get; set; }

        [NotMapped]
        public int PageIndex { get; set; }
    }
}
