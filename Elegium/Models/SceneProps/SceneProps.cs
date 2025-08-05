using Elegium.Models.Props;
using Elegium.Models.ScenesandScript;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.SceneProps
{
    public class SceneProps
    {
        public int Id { get; set; }
        public int SceneId { get; set; }
        [ForeignKey("SceneId")]
        public virtual Scene Scene { get; set; }
        public int PropsId { get; set; }
        [ForeignKey("PropsId")]
        public virtual Props.Props Props { get; set; }
    }
}
