using Elegium.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Voting
{
    public class Nomination
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ProductionType ProductionType { get; set; }
        public int? ProductionTypeId { get; set; }
        public Country Country { get; set; }
        public int? CountryId { get; set; }
        public string TermsAndConditions { get; set; }
        public Currency Currency { get; set; }
        public int? CurrencyId { get; set; }
        public float Price { get; set; }
        public bool IsVotingStarted { get; set; }
        public bool IsVotingFinished { get; set; }
        public bool IsResultApproved { get; set; }
        public DateTime ResultsApprovalTime { get; set; }
        public DateTime VotingStartDateTime { get; set; }
        public DateTime VotingFinishDateTime { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
