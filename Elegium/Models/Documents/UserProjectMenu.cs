using Elegium.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class UserProjectMenu
    {
        public int Id { get; set; }
        public virtual Project Project { get; set; }
        public int ProjectId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual MenuActivity MenuActivity { get; set; }
        public int MenuActivityId { get; set; }
    }
}
