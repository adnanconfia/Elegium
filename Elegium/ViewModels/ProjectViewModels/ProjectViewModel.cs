using Elegium.Models;
using Elegium.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.ViewModels.ProjectViewModels
{
    public class ProjectViewModel
    {
        public Project Project { get; set; }
        public ProjectPartnerRequirement ProjectPartnerRequirement { get; set; }
        public List<ProjectPartner> ProjectPartners { get; set; }
        public ProjectFunding ProjectFunding { get; set; }
        public ProjectFinancialParticipation ProjectFinancialParticipation { get; set; }
        public string[] ProjectVisibilityAreas { get; set; }

        public string ProjectLogo { get; set; }
        public string ProjectOriginalLogo { get; set; }

        //Management Phase Lists below
        //public string[] ProjectPartnerRequirementManagementPhases { get; set; }
        //public string[] ProjectFundingManagementPhases { get; set; }
        //public string[] ProjectFinancialParticipationManagementPhases { get; set; }

        //User info
        public UserProfile ProjectOwner { get; set; }

    }
}
