using Elegium.Models.Voting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos.Voting
{
    public class NominationVoteDto
    {
        public int Id { get; set; }
        public NominationDetail NominationDetail { get; set; }
        public int NominationDetailId { get; set; }
        public float TotalScore { get; set; }
        public string UserVotedName { get; set; }
        public string UserVotedLocation { get; set; }
        public string UserVotedId { get; set; }
        public List<NominationVoteDetailDto> NominationVoteDetails { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }

    public class NominationVoteDetailDto
    {
        public int Id { get; set; }
        public NominationVote NominationVote { get; set; }
        public int NominationVoteId { get; set; }
        public VotingParameter VotingParameter { get; set; }
        public int VotingParameterId { get; set; }
        public float Score { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
