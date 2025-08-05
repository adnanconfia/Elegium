using Elegium.Models;
using Elegium.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos.ProjectDtos
{
    public class ProjectFundingDto
    {
        public int Id { get; set; }
        public string Amount { get; set; }
        public Currency Currency { get; set; }
        public int? CurrencyId { get; set; }
        public string ProjectManagementPhaseId { get; set; }
        public string Requirements { get; set; }
        public FundersRequired FundersRequired { get; set; }
        public string FundersRequiredId { get; set; }
        public string BenefitsOffer { get; set; }
        public int ProjectId { get; set; }
    }
}
