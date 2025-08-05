using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Voting
{
    public class FinalVoteDetail
    {
        public int Id { get; set; }
        public FinalVote FinalVote { get; set; }
        public int FinalVoteId { get; set; }
        public VotingParameter VotingParameter { get; set; }
        public int VotingParameterId { get; set; }
        public float Score { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
    }
}
