using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Voting
{
    public class NominationVoteDetail
    {
        public int Id { get; set; }
        public NominationVote NominationVote { get; set; }
        public int NominationVoteId { get; set; }
        public VotingParameter VotingParameter { get; set; }
        public int VotingParameterId { get; set; }
        public float Score { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
    }
}
