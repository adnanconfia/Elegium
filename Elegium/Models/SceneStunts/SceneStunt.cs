using Elegium.Models.ScenesandScript;
using Elegium.Models.Stunts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.SceneStunts
{
    public class SceneStunt
    {
        public int Id { get; set; }
        public int SceneId { get; set; }
        [ForeignKey("SceneId")]
        public virtual Scene Scene { get; set; }
        public int StuntId { get; set; }
        [ForeignKey("StuntId")]
        public virtual Stunt Stunt { get; set; }
    }
}
