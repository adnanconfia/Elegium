using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Projects
{
    public class ProjectPartnerRequirementManagementPhase
    {
        public int Id { get; set; }
        public ProjectManagementPhases ProjectManagementPhases { get; set; }
        public string ProjectManagementPhasesId { get; set; }
        public ProjectPartnerRequirement ProjectPartnerRequirement { get; set; }
        public int ProjectPartnerRequirementId { get; set; }
    }
}
