using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class DocumentCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Documents Document { get; set; }
        public int DocumentId { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string Icon { get; set; }
        public bool CanView { get; set; }
        public bool CanEdit { get; set; }
        public bool Deleted { get; set; }
    }
}
