using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Calendar
{
    public class EventsAssignedTo
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public Event Event { get; set; }
        public int EventId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    }

    public class EventsAdditionalViewer
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public Event Event { get; set; }
        public int EventId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    }
}
