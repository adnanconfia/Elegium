using Elegium.Dtos.ResourceDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos
{
    public class ProjectResourceDto
    {
        public int Id { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public bool Available { get; set; } = true;
        public string OwnerId { get; set; }
        public string SenderId { get; set; }
        public int ProjectId { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public ResourceDto ResourceDto { get; set; }
        public string Status { get; set; }
        public string HireOrSale { get; set; }

        public ProjectDtos.ProjectDto ProjectDto { get; set; }
        public string SenderName { get; set; }
        public string Action { get; set; }

        public string PurchaseOn { get; set; }
    }
}
