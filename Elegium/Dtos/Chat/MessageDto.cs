using Elegium.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos.Chat
{
    public class MessageDto
    {
        public MessageDto()
        {
            Files = new List<string>();
            //fileMsgDto = new FileMessageDto();
        }
        public string Text { get; set; }
        public DateTime When { get; set; } = DateTime.UtcNow;
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public bool Delivered { get; set; }
        public bool Read { get; set; }
        public Guid? ThreadId { get; set; }
        public Guid? MessageId { get; set; }
        public bool IsItFromMe { get; set; }
        public string FriendlyTime { get; set; }
        public string SenderName { get; set; }
        public List<string> Files { get; set; }
        public string ReceiverName { get; set; }
        public int FilesCount { get; set; }
        public FileMessageDto fileMsgDto { get; set; }
    }
}
