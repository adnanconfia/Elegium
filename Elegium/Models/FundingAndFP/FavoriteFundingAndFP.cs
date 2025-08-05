using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.FundingAndFP
{
    public class FavoriteFundingAndFP
    {
        public int Id { get; set; }
        public UserFundingAndFP UserFundingAndFP { get; set; }
        public int UserFundingAndFPId { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
    }
}
