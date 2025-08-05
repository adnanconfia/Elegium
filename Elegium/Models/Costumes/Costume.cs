using Elegium.Models.Characters;
using Elegium.Models.Extras;
using Elegium.Models.Projects;
using Elegium.Models.ScenesandScript;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Costumes
{
    public class Costume
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Index { get; set; }
        
        
        public int? CharacterId { get; set; }

        [ForeignKey("CharacterId")]
        public virtual Character Character { get; set; }
        public int? ExtraId { get; set; }

        [ForeignKey("ExtraId")]
        public virtual Extra Extra { get; set; } 
        public int ProjectId { get; set; }

    }
}
