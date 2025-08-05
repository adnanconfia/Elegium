using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Overview
{
    public class DashboardPanel
    {
        public int Id { get; set; }

        public string PanelId { get; set; }
        public string PanelLabel { get; set; }
        public string EffectAllowed { get; set; }
    }

    public class DashboardSelectedPanel
    {
        public int Id { get; set; }
        public string PanelId { get; set; }
        public string PanelLabel { get; set; }
        public string EffectAllowed { get; set; }
        public int Sort { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}
