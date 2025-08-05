using Elegium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class ProjectTaskAssignedTo
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ProjectTask ProjectTask { get; set; }
        public int ProjectTaskId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    }
}
