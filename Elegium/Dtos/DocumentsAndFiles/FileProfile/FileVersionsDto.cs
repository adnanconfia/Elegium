using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos
{
    public class FileVersionsDto
    {
        public string ContentType { get; set; }
        public int DocumentFileId { get; set; }
        public string Extension { get; set; }
        public string FileId { get; set; }
        public int Id { get; set; }
        public string MimeType { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string UserFriendlySize { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string RelativeTime { get; set; }
        public string UserFriendlyTime { get; set; }
        public string Version { get; set; }
        public long CreateAtTicks { get; set; }
    }
}
