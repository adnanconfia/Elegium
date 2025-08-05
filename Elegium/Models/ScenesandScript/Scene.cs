using Elegium.Models.ProjectCrews;
using Elegium.Models.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.ScenesandScript
{
    public class Scene
    {
        
        public int Id { get; set; }
        public string Index { get; set; }
        public int project_id { get; set; }
        [ForeignKey("project_id")]
        public virtual Project Project { get; set; }
        public int? environment_id { get; set; }
        public string point_in_time { get; set; }
        public int? setId { get; set; }

        public string Description { get; set; }
        public string ScriptPages { get; set; }
        public int Estime_mm { get; set; }
        public int Estime_ss { get; set; }
        public int scheduled_hh { get; set; }
        public int scheduled_mm { get; set; }
        public string CastType { get; set; }
        public string  CommnityInformation { get; set; }
        public bool IsOmitted { get; set; }
        public int ScriptDay { get; set; }
        public int ScriptPage { get; set; }
        public int NumberOfShots { get; set; }
        public bool isDeleted { get; set; }
        public Nullable<int> unit { get; set; }
        [ForeignKey("unit")]
        public virtual ProjectUnit ProjectUnit { get; set; }

    }
}
