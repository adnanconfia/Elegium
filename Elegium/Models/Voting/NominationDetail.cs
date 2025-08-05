using Elegium.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Voting
{
    public class NominationDetail
    {
        public int Id { get; set; }
        public Nomination Nomination { get; set; }
        public int NominationId { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        public bool IsSelectedForFinalVoting { get; set; }
        public bool IsWinner { get; set; }
        public bool IsSystemSuggested { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
