using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elegium.Models.Projects;

namespace Elegium.Models.ProjectCrews
{
    public class ProjectCrewUnit
    {
        public int Id { get; set; }
        public ProjectCrew ProjectCrew { get; set; }
        public int ProjectCrewId { get; set; }
        public ProjectUnit Unit { get; set; }
        public int UnitId { get; set; }
    }
}
