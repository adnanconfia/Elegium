using Microsoft.CodeAnalysis.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Projects
{
    public class ProjectVisibilityArea
    {
        public int Id { get; set; }
        public VisibilityAreas VisibilityArea { get; set; }
        public string VisibilityAreaId { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
    }
}
