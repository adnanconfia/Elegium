using Elegium.Models.Extras;
using Elegium.Models.ScenesandScript;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.ScenesExtra
{
    public class ScenesExtra
    {
        public int Id { get; set; }
        public int SceneId { get; set; }
        [ForeignKey("SceneId")]
        public virtual Scene Scene { get; set; }
        public int ExtraId{ get; set; }

        [ForeignKey("ExtraId")]
        public virtual Extra Extra { get; set; }
    }
}
