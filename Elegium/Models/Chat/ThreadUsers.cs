using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Chat
{
    public class ThreadUsers
    {
        public Guid Id { get; set; }
        public virtual Thread Thread { get; set; }
        public virtual ApplicationUser User { get; set; }
        public Guid ThreadId { get; set; }
        public string UserId { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;


    }
}
