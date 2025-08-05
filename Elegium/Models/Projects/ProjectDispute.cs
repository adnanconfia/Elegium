using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Projects
{
    public class ProjectDispute
    {
        public int Id { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public DateTime DisputeDate { get; set; } = DateTime.UtcNow;
        public int Status { get; set; } //1=>Pending, 2=>Accepted, 3=>Rejected
    }
}
