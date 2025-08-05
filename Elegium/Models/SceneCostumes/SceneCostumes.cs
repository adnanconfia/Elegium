using Elegium.Models.Characters;
using Elegium.Models.Costumes;
using Elegium.Models.Extras;
using Elegium.Models.ScenesandScript;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.SceneCostumes
{
    public class SceneCostumes
    {
        public int Id { get; set; }
        public int SceneId { get; set; }
        [ForeignKey("SceneId")]
        public virtual Scene Scene { get; set; }
        public int CostumeId { get; set; }
        [ForeignKey("CostumeId")]
        public virtual Costume Costumes { get; set; }
        public int? CharacterId { get; set; }

        [ForeignKey("CharacterId")]
        public virtual Character Character { get; set; }
        public int? ExtraId { get; set; }

        [ForeignKey("ExtraId")]
        public virtual Extra Extra { get; set; }
        public int ProjectId { get; set; }
    }
}
