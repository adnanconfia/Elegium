using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Projects
{
    public class ProjectFinancialParticipationManagementPhase
    {
        public int Id { get; set; }
        public ProjectManagementPhases ProjectManagementPhases { get; set; }
        public string ProjectManagementPhasesId { get; set; }
        public ProjectFinancialParticipation ProjectFinancialParticipation { get; set; }
        public int ProjectFinancialParticipationId { get; set; }
    }
}
