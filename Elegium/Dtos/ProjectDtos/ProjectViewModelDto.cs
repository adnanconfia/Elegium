using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos.ProjectDtos
{
    public class ProjectViewModelDto
    {
        public ProjectDto Project { get; set; }
        public ProjectPartnerRequirementDto ProjectPartnerRequirement { get; set; }
        public List<ProjectPartnerDto> ProjectPartners { get; set; }
        public ProjectFundingDto ProjectFunding { get; set; }
        public ProjectFinancialParticipationDto ProjectFinancialParticipation { get; set; }
        
        //User info
        public UserProfileDto ProjectOwner { get; set; }
    }
}
