using Elegium.Models.Costumes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos.CostumeDto
{
    public class CostumeDto
    {
        public int Id { get; set; }
        public int? Index { get; set; }
        public string Name { get; set; }

        public int? CharacterId { get; set; }

   
        public int? ExtraId { get; set; }
        public int ProjectId { get; set; }

        public int sceneId { get; set; }
    }
}
