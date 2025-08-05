using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elegium.Models.Projects;

namespace Elegium.Models
{
    public class ProjectUserGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsInCrewList { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
    }
}
