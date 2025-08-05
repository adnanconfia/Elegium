using Elegium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class FileTaskAssignedTo
    {
        public int Id { get; set; }
        //public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public FileTask ProjectTask { get; set; }
        public int FileTaskId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    }
}
