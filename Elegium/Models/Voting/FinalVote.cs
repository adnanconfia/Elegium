using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Voting
{
    public class FinalVote
    {
        public int Id { get; set; }
        public NominationDetail NominationDetail { get; set; }
        public int NominationDetailId { get; set; }
        public float TotalScore { get; set; }
        public ApplicationUser UserVoted { get; set; }
        public string UserVotedId { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
    }
}
