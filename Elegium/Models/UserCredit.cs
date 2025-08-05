using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class UserCredit
    {
        public int Id { get; set; }
        public string Year { get; set; }
        public string Workplace { get; set; }
        public string Job { get; set; }

        //Link to the application user
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}
