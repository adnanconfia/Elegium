using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Projects
{
    public class ProjectPartnerRequirement
    {
        public int Id { get; set; }
        public int Budget { get; set; }
        public string YourFinancialShare { get; set; }
        public ProjectManagementPhases ProjectManagementPhase { get; set; }
        public string ProjectManagementPhaseId { get; set; }
        public int? ProjectPartnersCount { get; set; }
        public bool SynopsisCompleted { get; set; }
        public bool PlotCompleted { get; set; }
        public bool ScreenplayCompleted { get; set; }
        public bool ScreenplayWorkRequired { get; set; }
        public bool NeedFinancialParticipation { get; set; }

        public Project Project { get; set; }
        public int ProjectId { get; set; }

    }
}
