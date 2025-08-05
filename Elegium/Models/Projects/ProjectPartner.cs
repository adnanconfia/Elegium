using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Projects
{
    public class ProjectPartner
    {
        public int Id { get; set; }
        public string ProjectPartnerRole { get; set; }
        public bool FinancialParticipationRequired { get; set; }
        public string FinancialShare { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        public UserProfile PartnerUser { get; set; }
        public int? PartnerUserId { get; set; }
    }
}
