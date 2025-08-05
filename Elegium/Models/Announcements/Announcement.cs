using Elegium.Interfaces;
using Elegium.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class Announcement
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public bool PinTop { get; set; }
        public bool HasDeadline { get; set; }
        public DateTime? Deadline { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        public bool Deleted { get; set; }
        public bool IncludeExternal { get; set; } = true;
    }
}
