using Elegium.Models.ScenesandScript;
using Elegium.Models.VisualEffects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.SceneVisuals
{
    public class SceneVisual
    {
        public int Id { get; set; }
        public int SceneId { get; set; }
        [ForeignKey("SceneId")]
        public virtual Scene Scene { get; set; }
        public int VisualId { get; set; }
        [ForeignKey("VisualId")]
        public virtual VisualEffect VisualEffect { get; set; }
    }
}
