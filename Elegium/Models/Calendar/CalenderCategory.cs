using Elegium.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Calendar
{
    public class CalenderCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
