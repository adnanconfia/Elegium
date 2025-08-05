using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.ProjectCrews
{
    public class ProjectCrewGroup
    {
        public int Id { get; set; }
        public ProjectCrew ProjectCrew { get; set; }
        public int ProjectCrewId { get; set; }
        public ProjectUserGroup Group { get; set; }
        public int GroupId { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public ApplicationUser? User { get; set; }
        public string UserId { get; set; }
    }
}
