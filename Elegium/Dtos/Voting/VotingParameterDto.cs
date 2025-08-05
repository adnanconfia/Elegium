using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos.Voting
{
    public class VotingParameterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float MinScore { get; set; }
        public float MaxScore { get; set; }
        public float Score { get; set; }
        public bool IsActive { get; set; }
    }
}
