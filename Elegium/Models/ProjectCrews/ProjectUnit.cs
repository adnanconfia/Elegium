using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elegium.Models.Projects;

namespace Elegium.Models.ProjectCrews
{
    public class ProjectUnit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Project Project { get; set; }
        public int? ProjectId { get; set; }
    }
}
