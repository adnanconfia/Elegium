using Elegium.Models.Actor;
using Elegium.Models.Characters;
using Elegium.Models.Extras;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace Elegium.Models.CharacterTalent
{
    public class CharactersTalent
    {
        public int Id { get; set; }

        public int Rating { get; set; } = 0;
        public bool Is_CastFixed { get; set; } = false;
        public bool Is_Rejected { get; set; } = false;

        

        public int? ActorId { get; set; }
        [ForeignKey("ActorId")]
        public virtual Actors Actor { get; set; }
        public int? ExtraId { get; set; }
        [ForeignKey("ExtraId")]
        public virtual Extra Extra { get; set; }
        public int? CharId { get; set; }
        [ForeignKey("CharId")]
        public virtual Character Character { get; set; }
        public int? TalentId { get; set; }
        [ForeignKey("TalentId")]
        public virtual Talents Talent { get; set; }


    }
}
