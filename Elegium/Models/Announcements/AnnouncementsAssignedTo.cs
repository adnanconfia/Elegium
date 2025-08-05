using Elegium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class AnnouncementsAssignedTo
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public Announcement Announcement { get; set; }
        public int AnnouncementId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    }
}
