using Elegium.Models;
using Elegium.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos.ProjectDtos
{
    public class ProjectDisputeDto
    {
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        public string Description { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public DateTime DisputeDate { get; set; }
        public int Status { get; set; }
        public List<ProjectDisputeDetail> ProjectDisputeDetails { get; set; }
    }
}
