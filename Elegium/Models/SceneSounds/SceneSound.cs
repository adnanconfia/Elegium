using Elegium.Models.ScenesandScript;
using Elegium.Models.Sounds;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.SceneSounds
{
    public class SceneSound
    {
        public int Id { get; set; }
        public int SceneId { get; set; }
        [ForeignKey("SceneId")]
        public virtual Scene Scene { get; set; }
        public int SoundId { get; set; }
        [ForeignKey("SoundId")]
        public virtual Sound Sounds { get; set; }
    }
}
