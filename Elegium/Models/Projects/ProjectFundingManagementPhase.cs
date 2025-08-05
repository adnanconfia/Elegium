using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Projects
{
    public class ProjectFundingManagementPhase
    {
        public int Id { get; set; }
        public ProjectManagementPhases ProjectManagementPhases { get; set; }
        public string ProjectManagementPhasesId { get; set; }
        public ProjectFunding ProjectFunding { get; set; }
        public int ProjectFundingId { get; set; }
    }
}
