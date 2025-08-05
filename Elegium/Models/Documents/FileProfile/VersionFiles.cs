using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class VersionFiles
    {
        public string ContentType { get; set; }
        public int DocumentFileId { get; set; }
        public string FileId { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public int Id { get; set; }
        public DocumentFiles DocumentFile { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public string MimeType { get; set; }
        public string Extension { get; set; }
        public string Type { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public string UserFriendlySize { get; set; }
        public long CreateAtTicks { get; set; } = DateTime.UtcNow.Ticks;
        public string Version { get; set; }
    }
}
