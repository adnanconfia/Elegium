using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos.MakeupDto
{
    public class MakeupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? CharacterId { get; set; }


        public int? ExtraId { get; set; }
        public int ProjectId { get; set; }

        public int sceneId { get; set; }
    }
}
