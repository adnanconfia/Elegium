using Elegium.Models.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Links
{
    public class link
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool Is_Deleted { get; set; } = false;
        public Actors Actor { get; set; }
        public int? ActorId { get; set; }
        public Talents Talent { get; set; }
        public int? TalentId { get; set; }
    }
}
