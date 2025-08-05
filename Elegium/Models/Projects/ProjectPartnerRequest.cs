using Elegium.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class ProjectPartnerRequest
    {
        public int Id { get; set; }
        public ApplicationUser Sender { get; set; }
        public string SenderId { get; set; }
        public ApplicationUser Owner { get; set; }
        public string OwnerId { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public string Description { get; set; }
    }
}
