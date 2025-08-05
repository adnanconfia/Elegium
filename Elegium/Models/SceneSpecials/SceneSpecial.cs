using Elegium.Models.ScenesandScript;
using Elegium.Models.SpecialEffects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.SceneSpecials
{
    public class SceneSpecial
    {
        public int Id { get; set; }
        public int SceneId { get; set; }
        [ForeignKey("SceneId")]
        public virtual Scene Scene { get; set; }
        public int SpecialId { get; set; }
        [ForeignKey("SpecialId")]
        public virtual SpecialEffect SpecialEffect { get; set; }
    }
}
