using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos
{
    public class ProjectPartnerRequestDto
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string SenderId { get; set; }
        public string Owner { get; set; }
        public string OwnerId { get; set; }
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
        public string Status { get; set; }
        public string Created { get; set; }
        public string Description { get; set; }
        public string Action { get; set; }
    }
}
