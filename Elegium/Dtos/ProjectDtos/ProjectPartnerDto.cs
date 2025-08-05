using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos.ProjectDtos
{
    public class ProjectPartnerDto
    {
        public int Id { get; set; }
        public string ProjectPartnerRole { get; set; }
        public bool FinancialParticipationRequired { get; set; }
        public string FinancialShare { get; set; }
        public int ProjectId { get; set; }
        public UserProfileDto PartnerUser { get; set; }
        public int? PartnerUserId { get; set; }
    }
}
