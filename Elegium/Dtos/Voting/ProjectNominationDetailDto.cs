using Elegium.Models.Voting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos.Voting
{
    public class ProjectNominationDetailDto
    {
        public int Id { get; set; }
        public int NominationId { get; set; }
        public string NominationName { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDetail { get; set; }
        public int ProjectId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Location { get; set; }
        public float TotalScore { get; set; }
        public float AverageScore { get; set; }
        public int UsersVotedCount { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool AlreadyVoted { get; set; }
        public bool IsSelectedForFinalVoting { get; set; }
        public bool IsWinner { get; set; }
        public bool IsSystemSuggested { get; set; }
        public List<FinalVoteDetailDto> FinalVoteDetails { get; set; }
        public List<VotingParameterDto> VotingParameters { get; set; }
        public DateTime AppliedDateTime { get; set; }
    }
}
