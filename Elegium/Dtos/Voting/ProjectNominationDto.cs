using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos.Voting
{
    public class ProjectNominationDto
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Location { get; set; }
        public DateTime AppliedDateTime { get; set; }
    }
}
