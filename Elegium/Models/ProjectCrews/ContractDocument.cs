using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.ProjectCrews
{
    public class ContractDocument
    {
        public string ContentType { get; set; }
        public string FileId { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public int Id { get; set; }
        public ProjectCrew ProjectCrew { get; set; }
        public int ProjectCrewId { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public string MimeType { get; set; }
        public string Extension { get; set; }
        public string Type { get; set; }
        public string UserFriendlySize { get; set; }
        public long CreateAtTicks { get; set; } = DateTime.UtcNow.Ticks;
        public bool Default { get; set; }
    }
}
