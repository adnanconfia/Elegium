using Elegium.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Professionals
{
    public class ProfessionalHireRequest
    {
        public int Id { get; set; }
        public ApplicationUser Professional { get; set; }
        public string ProfessionalId { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        public WorkingPosition WorkingPosition { get; set; }
        public int? WorkingPositionId { get; set; }
        public string Message { get; set; }
        public DateTime RequestDateTime { get; set; } = DateTime.UtcNow;
        public int Status { get; set; } //1 => Requested, 2 => Accepted, 3 => Rejected
        public DateTime StatusDateTime { get; set; }
    }
}
