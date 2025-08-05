using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Chat
{
    public class MessageFiles
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileId { get; set; }
        public string Type { get; set; } //P-> Photos, V-> Videos, A -> Audios
        public string ContentType { get; set; }
        public int Size { get; set; }
        public  virtual Message Message  { get; set; }
        public Guid MessageId { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public long CreatedAtTicks { get; set; } = DateTime.UtcNow.Ticks;
    }
}
