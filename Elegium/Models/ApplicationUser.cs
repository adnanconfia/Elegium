using Elegium.Models.Chat;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
         //   Messages = new HashSet<Message>();
            Connections = new HashSet<Connection>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Industry { get; set; }
        public bool Banned { get; set; } = false;
        public bool Active { get; set; } = true;

        public bool Trash { get; set; } = false;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        //public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Connection> Connections { get; set; }
    }
}
