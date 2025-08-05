using Elegium.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class FileTask: ITask
    {
        public int  Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool HasDeadline { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public virtual DocumentFiles DocumentFiles { get; set; }
        public int DocumentFilesId { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public bool Completed { get; set; }
    }
}
