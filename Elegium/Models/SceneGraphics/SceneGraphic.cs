using Elegium.Models.Graphics;
using Elegium.Models.ScenesandScript;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.SceneGraphics
{
    public class SceneGraphic
    {

        public int Id { get; set; }
        public int SceneId { get; set; }
        [ForeignKey("SceneId")]
        public virtual Scene Scene { get; set; }
        public int GraphicId { get; set; }
        [ForeignKey("GraphicId")]
        public virtual Graphic Graphics { get; set; }
    }
}
