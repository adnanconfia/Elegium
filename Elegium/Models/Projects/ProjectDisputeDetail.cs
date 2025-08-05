using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Projects
{
    public class ProjectDisputeDetail
    {
        public int Id { get; set; }
        public ProjectDispute ProjectDispute { get; set; }
        public int ProjectDisputeId { get; set; }
        public DateTime EnteryDate { get; set; } = DateTime.UtcNow;
        public string Description { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}
