using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Elegium.Models.Projects
{
    public class ProjectFunding
    {
        public int Id { get; set; }
        public string Amount { get; set; }
        public Currency Currency { get; set; }
        public int? CurrencyId { get; set; }
        public ProjectManagementPhases ProjectManagementPhase { get; set; }
        public string ProjectManagementPhaseId { get; set; }
        public string Requirements { get; set; }
        public FundersRequired FundersRequired { get; set; }
        public string FundersRequiredId { get; set; }
        public string BenefitsOffer { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }

    }
}
