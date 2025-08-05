using Elegium.Models.Others;
using Elegium.Models.ScenesandScript;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.SceneOthers
{
    public class SceneOther
    {
        public int Id { get; set; }
        public int SceneId { get; set; }
        [ForeignKey("SceneId")]
        public virtual Scene Scene { get; set; }
        public int OtherId { get; set; }
        [ForeignKey("OtherId")]
        public virtual other Other { get; set; }
    }
}
