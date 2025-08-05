using Elegium.Models.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.OffPeriods
{
    public class offperiod
    {
        public int Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }  
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public string Note { get; set; }
        public bool Is_Deleted { get; set; } = false;
        public Actors Actor { get; set; }
        public int? ActorId { get; set; }
        public Talents Talent { get; set; }
        public int? TalentId { get; set; }
    }
}
