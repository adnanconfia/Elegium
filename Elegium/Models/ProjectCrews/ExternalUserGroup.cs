using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.ProjectCrews
{
    public class ExternalUserGroup
    {
        public int Id { get; set; }
        public ProjectExternalUser ExternalUser { get; set; }
        public int ExternalUserId { get; set; }
        public ProjectUserGroup Group { get; set; }
        public int GroupId { get; set; }
    }
}
