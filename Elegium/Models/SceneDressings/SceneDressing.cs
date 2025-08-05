using Elegium.Models.Dressing;
using Elegium.Models.ScenesandScript;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.SceneDressings
{
    public class SceneDressing
    {
        public int Id { get; set; }
        public int SceneId { get; set; }
        [ForeignKey("SceneId")]
        public virtual Scene Scene { get; set; }
        public int DressingId { get; set; }
        [ForeignKey("DressingId")]
        public virtual dressing Dressing { get; set; }
    }
}
