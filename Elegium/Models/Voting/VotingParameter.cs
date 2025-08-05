using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Voting
{
    public class VotingParameter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float MinScore { get; set; }
        public float MaxScore { get; set; }
        public bool IsActive { get; set; }
    }
}
