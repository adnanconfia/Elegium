using Elegium.Models.Projects;
using Elegium.Models.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class ProjectResource
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Project Project { get; set; }
        public int? ProjectId { get; set; }
        public string HireOrSale { get; set; }
        public DateTime? Created { get; set; } = DateTime.UtcNow;
        public DateTime? FromTime { get; set; }
        public DateTime? ToTime { get; set; }
        public ApplicationUser Sender { get; set; }
        public ApplicationUser Owner { get; set; }
        public string SenderId { get; set; }
        public string OwnerId { get; set; }
        public double Price { get; set; }
        public Resource Resource { get; set; }
        public int? ResourceId { get; set; }
        public string Status { get; set; } //rejected or accepted or pending or Completed R or A or P respectively
        public int? OriginalResourceId { get; set; }
        public Resource OriginalResource { get; set; }
        public DateTime? PurchaseOn { get; set; }
    }
}
