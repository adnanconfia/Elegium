using Elegium.Models.Cameras;
using Elegium.Models.ScenesandScript;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.SceneCameras
{
    public class SceneCamera
    {
        public int Id { get; set; }
        public int SceneId { get; set; }
        [ForeignKey("SceneId")]
        public virtual Scene Scene { get; set; }
        public int CameraId { get; set; }
        [ForeignKey("CameraId")]
        public virtual Camera Camera { get; set; }
    }
}
