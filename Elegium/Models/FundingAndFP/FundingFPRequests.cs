using Elegium.Dtos.ProjectDtos;
using Elegium.Models.Projects;
using Elegium.ViewModels.FundingAndFP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class FundingFPRequests
    {
        public int Id { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        public string FundingOrFP { get; set; } // F for Funding.... FP for financial Participation....
        public int? Offer { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public ApplicationUser Sender { get; set; }
        public string SenderId { get; set; }
        public ApplicationUser Owner { get; set; }
        public string OwnerId { get; set; }
        public string Status { get; set; }
        public string OfferOrLooking { get; set; }

        [NotMapped]
        public ProjectViewModelDto dto { get; set; }
        [NotMapped]
        public UserFundingAndFPDto UserFundingAndFPDto { get; set; }
    }
}
