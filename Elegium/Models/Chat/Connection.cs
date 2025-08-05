using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Chat
{
    public class Connection
    {
        public string ConnectionID { get; set; }
        public string UserAgent { get; set; }
        public bool Connected { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public DateTime ConnectionTime { get; set; } = DateTime.UtcNow;
    }
}
