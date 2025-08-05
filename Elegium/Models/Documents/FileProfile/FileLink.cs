using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class FileLink
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public virtual DocumentFiles DocumentFile { get; set; }
        public int DocumentFileId { get; set; }
    }
}
