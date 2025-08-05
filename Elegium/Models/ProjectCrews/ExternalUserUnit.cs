using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elegium.Models.Projects;

namespace Elegium.Models.ProjectCrews
{
    public class ExternalUserUnit
    {
        public int Id { get; set; }
        public ProjectExternalUser ExternalUser { get; set; }
        public int ExternalUserId { get; set; }

        public ProjectUnit Unit { get; set; }
        public int UnitId { get; set; }
    }
}
