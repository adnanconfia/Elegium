using Elegium.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Calendar
{
    public class Event
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; } 
        public string Color { get; set; }
        public string Location { get; set; }
        public bool AllDay { get; set; }
        public CalenderCategory CalenderCategory { get; set; }
        public int? CalenderCategoryId { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public bool Deleted { get; set; }
    }
}
