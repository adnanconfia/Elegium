using Elegium.Models.Construction;
using Elegium.Models.ScenesandScript;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.SceneConstruction
{
    public class SceneConstruction
    {
        public int Id { get; set; }
        public int SceneId { get; set; }
        [ForeignKey("SceneId")]
        public virtual Scene Scene { get; set; }
        public int ConstructionId { get; set; }
        [ForeignKey("ConstructionId")]
        public virtual construction Construction {get;set;}

    }
}
