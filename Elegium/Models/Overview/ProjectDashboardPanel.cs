using Elegium.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Overview
{
    public class ProjectDashboardPanel
    {
        public int Id { get; set; }
        public string PanelId { get; set; }
        public string PanelLabel { get; set; }
        public string EffectAllowed { get; set; }
    }

    public class ProjectDashboardSelectedPanel
    {
        public int Id { get; set; }
        public string PanelId { get; set; }
        public string PanelLabel { get; set; }
        public string EffectAllowed { get; set; }
        public int Sort { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}
